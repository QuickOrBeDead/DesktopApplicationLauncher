namespace DesktopApplicationLauncher.Wpf.ComponentTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;

    using Models;
    using Infrastructure.Business;
    using Infrastructure.Data;
    using Infrastructure.Entities;
    using DesktopApplicationLauncher.Wpf.Infrastructure.Models;

    using LiteDB;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public sealed class ApplicationServiceStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        private ApplicationService _applicationService;
        private IDbContext _liteDbContext;

        public ApplicationServiceStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Before]
        public void Before()
        {
            var liteDatabase = new LiteDatabase("Filename=ComponentTests.db");
            foreach (var collectionName in liteDatabase.GetCollectionNames())
            {
                liteDatabase.DropCollection(collectionName);
            }

            _liteDbContext = new LiteDbContext(liteDatabase);
            _applicationService = new ApplicationService(_liteDbContext);
        }
        
        [After]
        public void After()
        {
            _liteDbContext.Dispose();
        }

        [Given(@"the following folder structure is")]
        public void GivenTheFollowingFolderStructureIs(Table expected)
        {
            SaveFolders(CreateFolderTree(expected));

            var actual = GetApplicationsTable();
            expected.CompareToSet(actual, true);
        }


        [When(@"'(.*)' folder is moved to '(.*)'")]
        public void WhenFolderIsMovedTo(string sourceFolder, string targetFolder)
        {
            var sourceId = GetFolderIdByName(sourceFolder);
            var targetId = GetFolderIdByName(targetFolder);

            _applicationService.MoveToFolder(sourceId, targetId);
        }

        [When(@"'(.*)' file is renamed to '(.*)'")]
        public void WhenFileIsRenamedTo(string sourceFileName, string newFileName)
        {
            var source = GetApplicationByName(sourceFileName);

            _applicationService.UpdateApplication(new ApplicationUpdateModel
            {
                Id = source.Id,
                Name = newFileName,
                ParentId = source.ParentId,
                Arguments = source.Arguments,
                Description = source.Description,
                Path = source.Path
            });
        }

        [Then(@"the result should be")]
        public void ThenTheResultShouldBe(Table expected)
        {
            var actual = GetApplicationsTable();
            expected.CompareToSet(actual, true);
        }

        private void SaveNode(Node<(string Name, ApplicationItemType ItemType)> node, int? parentId = null)
        {
            var parent = _applicationService.SaveApplication(new ApplicationSaveModel
                                                                 {
                                                                     Name = node.Item.Name,
                                                                     ItemType = node.Item.ItemType,
                                                                     ParentId = parentId
                                                                 });
            var childNodes = node.Children;
            for (var j = 0; j < childNodes.Count; j++)
            {
                var childNode = childNodes[j];
                SaveNode(childNode, parent);
            }
        }

        private IList<ApplicationModel> GetApplicationsTable()
        {
            var applications = _liteDbContext.Applications.List()
                .Select(x => (x.Id, x.Name, x.ParentId, x.HierarchyPath, x.ItemType, x.SortOrder)).ToList();
            var nodes = new List<Node<(int Id, string Name, ApplicationItemType? ItemType, int? ParentId, string HierarchyPath)>>();
            var rootItems = applications.Where(x => x.ParentId == null).OrderBy(x => x.SortOrder).ToList();
            for (var i = 0; i < rootItems.Count; i++)
            {
                var rootItem = rootItems[i];
                var node = new Node<(int Id, string Name, ApplicationItemType? ItemType, int? ParentId, string HierarchyPath)>((rootItem.Id, rootItem.Name, rootItem.ItemType, rootItem.ParentId, rootItem.HierarchyPath));
                nodes.Add(node);

                AddChildNodes(applications, node);
            }

            var actual = new List<ApplicationModel>();
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];

                AddChildNodes(actual, node);
            }

            return actual;
        }

        private static void AddChildNodes(IList<ApplicationModel> items, Node<(int Id, string Name, ApplicationItemType? ItemType, int? ParentId, string HierarchyPath)> node, int level = 0)
        {
            items.Add(new ApplicationModel
                          {
                              Id = node.Item.Id,
                              Name = level == 0 ? node.Item.Name : $"{new string('.', level)} {node.Item.Name}",
                              Type = node.Item.ItemType == ApplicationItemType.Folder ? "Folder" : "File",
                              HierarchyPath = node.Item.HierarchyPath,
                              ParentId = node.Item.ParentId
                          });

            var children = node.Children;
            for (var j = 0; j < children.Count; j++)
            {
                var child = children[j];
                AddChildNodes(items, child, level + 1);
            }
        }

        private static void AddChildNodes(
            IList<(int Id, string Name, int? ParentId, string HierarchyPath, ApplicationItemType? ItemType, int SortOrder)> applications,
            Node<(int Id, string Name, ApplicationItemType? ItemType, int? ParentId, string HierarchyPath)> node)
        {
            var children = applications.Where(x => x.ParentId == node.Item.Id).OrderBy(x => x.SortOrder).ToList();
            for (var j = 0; j < children.Count; j++)
            {
                var child = children[j];
                var childNode = new Node<(int Id, string Name, ApplicationItemType? ItemType, int? ParentId, string HierarchyPath)>((child.Id, child.Name, child.ItemType, child.ParentId, child.HierarchyPath));
                node.Children.Add(childNode);

                AddChildNodes(applications, childNode);
            }
        }

        private void SaveFolders(IList<Node<(string Name, ApplicationItemType ItemType)>> tree)
        {
            for (var i = 0; i < tree.Count; i++)
            {
                var node = tree[i];
                SaveNode(node);
            }
        }

        private static IList<Node<(string Name, ApplicationItemType ItemType)>> CreateFolderTree(Table table)
        {
            IList<Node<(string Name, ApplicationItemType ItemType)>> nodes = new List<Node<(string Name, ApplicationItemType ItemType)>>();
            var stack = new Stack<Node<(string Name, ApplicationItemType ItemType)>>();

            var prevLevel = 0;
            var rows = table.Rows;
            for (var i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var name = row["Name"];
                var type = row["Type"];

                var applicationType = type == "Folder" ? ApplicationItemType.Folder : ApplicationItemType.File;
                var currentLevel = name.Count(x => x == '.');
                var node = new Node<(string Name, ApplicationItemType ItemType)>((name.Trim(' ', '.'), applicationType));
                if (currentLevel == prevLevel)
                {
                    if (stack.Count > 0)
                    {
                        stack.Pop();
                    }
                }
                else if (currentLevel < prevLevel)
                {
                    var difference = prevLevel - currentLevel;
                    for (var j = 0; j <= difference; j++)
                    {
                        if (stack.Count > 0)
                        {
                            stack.Pop();
                        }
                    }
                }

                if (stack.Count > 0)
                {
                    stack.Peek().Children.Add(node);
                }
                else
                {
                    nodes.Add(node);
                }

                stack.Push(node);

                prevLevel = currentLevel;
            }

            stack.Clear();

            return nodes;
        }

        private int GetFolderIdByName(string name)
        {
            return _liteDbContext.Applications.ListDto(x => x.Id, x => x.Name == name).SingleOrDefault();
        }

        private Application GetApplicationByName(string name)
        {
            return _liteDbContext.Applications.ListDto(x => x, x => x.Name == name).SingleOrDefault();
        }
    }
}
