﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace DesktopApplicationLauncher.Wpf.ComponentTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("MoveFolder")]
    public partial class MoveFolderFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "MoveFolder.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "MoveFolder", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Move folder to Same-level folder")]
        public virtual void MoveFolderToSame_LevelFolder()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Move folder to Same-level folder", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 3
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "Name",
                            "Type",
                            "ParentId",
                            "HierarchyPath"});
                table1.AddRow(new string[] {
                            "1",
                            "File1",
                            "File",
                            "",
                            ""});
                table1.AddRow(new string[] {
                            "2",
                            "Folder1",
                            "Folder",
                            "",
                            ""});
                table1.AddRow(new string[] {
                            "3",
                            ". File2",
                            "File",
                            "2",
                            "/2/"});
                table1.AddRow(new string[] {
                            "4",
                            ". Folder2",
                            "Folder",
                            "2",
                            "/2/"});
                table1.AddRow(new string[] {
                            "5",
                            ".. Folder3",
                            "Folder",
                            "4",
                            "/2/4/"});
                table1.AddRow(new string[] {
                            "6",
                            ".. File3",
                            "File",
                            "4",
                            "/2/4/"});
                table1.AddRow(new string[] {
                            "7",
                            "... Folder4",
                            "Folder",
                            "6",
                            "/2/4/6/"});
                table1.AddRow(new string[] {
                            "8",
                            ". File4",
                            "File",
                            "2",
                            "/2/"});
                table1.AddRow(new string[] {
                            "9",
                            ". Folder5",
                            "Folder",
                            "2",
                            "/2/"});
#line 4
 testRunner.Given("the following folder structure is", ((string)(null)), table1, "Given ");
#line hidden
#line 15
 testRunner.When("\'Folder2\' folder is moved to \'Folder5\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "Name",
                            "Type",
                            "ParentId",
                            "HierarchyPath"});
                table2.AddRow(new string[] {
                            "1",
                            "File1",
                            "File",
                            "",
                            ""});
                table2.AddRow(new string[] {
                            "2",
                            "Folder1",
                            "Folder",
                            "",
                            ""});
                table2.AddRow(new string[] {
                            "3",
                            ". File2",
                            "File",
                            "2",
                            "/2/"});
                table2.AddRow(new string[] {
                            "8",
                            ". File4",
                            "File",
                            "2",
                            "/2/"});
                table2.AddRow(new string[] {
                            "9",
                            ". Folder5",
                            "Folder",
                            "2",
                            "/2/"});
                table2.AddRow(new string[] {
                            "4",
                            ".. Folder2",
                            "Folder",
                            "9",
                            "/2/9/"});
                table2.AddRow(new string[] {
                            "5",
                            "... Folder3",
                            "Folder",
                            "4",
                            "/2/9/4/"});
                table2.AddRow(new string[] {
                            "6",
                            "... File3",
                            "File",
                            "4",
                            "/2/9/4/"});
                table2.AddRow(new string[] {
                            "7",
                            ".... Folder4",
                            "Folder",
                            "6",
                            "/2/9/4/6/"});
#line 16
 testRunner.Then("the result should be", ((string)(null)), table2, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Move folder to Lower-level folder")]
        public virtual void MoveFolderToLower_LevelFolder()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Move folder to Lower-level folder", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 28
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "Name",
                            "Type",
                            "ParentId",
                            "HierarchyPath"});
                table3.AddRow(new string[] {
                            "1",
                            "File1",
                            "File",
                            "",
                            ""});
                table3.AddRow(new string[] {
                            "2",
                            "Folder1",
                            "Folder",
                            "",
                            ""});
                table3.AddRow(new string[] {
                            "3",
                            ". File2",
                            "File",
                            "2",
                            "/2/"});
                table3.AddRow(new string[] {
                            "4",
                            ". Folder2",
                            "Folder",
                            "2",
                            "/2/"});
                table3.AddRow(new string[] {
                            "5",
                            ".. File3",
                            "File",
                            "4",
                            "/2/4/"});
                table3.AddRow(new string[] {
                            "6",
                            ". Folder3",
                            "Folder",
                            "2",
                            "/2/"});
                table3.AddRow(new string[] {
                            "7",
                            ".. Folder4",
                            "Folder",
                            "6",
                            "/2/6/"});
                table3.AddRow(new string[] {
                            "8",
                            ".. File4",
                            "File",
                            "6",
                            "/2/6/"});
                table3.AddRow(new string[] {
                            "9",
                            "... Folder5",
                            "Folder",
                            "8",
                            "/2/6/8/"});
                table3.AddRow(new string[] {
                            "10",
                            ".... File5",
                            "File",
                            "9",
                            "/2/6/8/9/"});
                table3.AddRow(new string[] {
                            "11",
                            ". File6",
                            "File",
                            "2",
                            "/2/"});
                table3.AddRow(new string[] {
                            "12",
                            ". Folder6",
                            "Folder",
                            "2",
                            "/2/"});
#line 29
 testRunner.Given("the following folder structure is", ((string)(null)), table3, "Given ");
#line hidden
#line 43
 testRunner.When("\'Folder2\' folder is moved to \'Folder5\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "Name",
                            "Type",
                            "ParentId",
                            "HierarchyPath"});
                table4.AddRow(new string[] {
                            "1",
                            "File1",
                            "File",
                            "",
                            ""});
                table4.AddRow(new string[] {
                            "2",
                            "Folder1",
                            "Folder",
                            "",
                            ""});
                table4.AddRow(new string[] {
                            "3",
                            ". File2",
                            "File",
                            "2",
                            "/2/"});
                table4.AddRow(new string[] {
                            "6",
                            ". Folder3",
                            "Folder",
                            "2",
                            "/2/"});
                table4.AddRow(new string[] {
                            "7",
                            ".. Folder4",
                            "Folder",
                            "6",
                            "/2/6/"});
                table4.AddRow(new string[] {
                            "8",
                            ".. File4",
                            "File",
                            "6",
                            "/2/6/"});
                table4.AddRow(new string[] {
                            "9",
                            "... Folder5",
                            "Folder",
                            "8",
                            "/2/6/8/"});
                table4.AddRow(new string[] {
                            "10",
                            ".... File5",
                            "File",
                            "9",
                            "/2/6/8/9/"});
                table4.AddRow(new string[] {
                            "4",
                            ".... Folder2",
                            "Folder",
                            "9",
                            "/2/6/8/9/"});
                table4.AddRow(new string[] {
                            "5",
                            "..... File3",
                            "File",
                            "4",
                            "/2/6/8/9/4/"});
                table4.AddRow(new string[] {
                            "11",
                            ". File6",
                            "File",
                            "2",
                            "/2/"});
                table4.AddRow(new string[] {
                            "12",
                            ". Folder6",
                            "Folder",
                            "2",
                            "/2/"});
#line 44
 testRunner.Then("the result should be", ((string)(null)), table4, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Move folder to Upper-level folder")]
        public virtual void MoveFolderToUpper_LevelFolder()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Move folder to Upper-level folder", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 59
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "Name",
                            "Type",
                            "ParentId",
                            "HierarchyPath"});
                table5.AddRow(new string[] {
                            "1",
                            "File1",
                            "File",
                            "",
                            ""});
                table5.AddRow(new string[] {
                            "2",
                            "Folder1",
                            "Folder",
                            "",
                            ""});
                table5.AddRow(new string[] {
                            "3",
                            ". File2",
                            "File",
                            "2",
                            "/2/"});
                table5.AddRow(new string[] {
                            "4",
                            ". Folder2",
                            "Folder",
                            "2",
                            "/2/"});
                table5.AddRow(new string[] {
                            "5",
                            ".. File3",
                            "File",
                            "4",
                            "/2/4/"});
                table5.AddRow(new string[] {
                            "6",
                            ". Folder3",
                            "Folder",
                            "2",
                            "/2/"});
                table5.AddRow(new string[] {
                            "7",
                            ".. Folder4",
                            "Folder",
                            "6",
                            "/2/6/"});
                table5.AddRow(new string[] {
                            "8",
                            "... File4",
                            "File",
                            "7",
                            "/2/6/7/"});
                table5.AddRow(new string[] {
                            "9",
                            "... Folder5",
                            "Folder",
                            "7",
                            "/2/6/7/"});
                table5.AddRow(new string[] {
                            "10",
                            ".... File5",
                            "File",
                            "9",
                            "/2/6/7/9/"});
                table5.AddRow(new string[] {
                            "11",
                            ". File6",
                            "File",
                            "2",
                            "/2/"});
                table5.AddRow(new string[] {
                            "12",
                            ". Folder6",
                            "Folder",
                            "2",
                            "/2/"});
#line 60
 testRunner.Given("the following folder structure is", ((string)(null)), table5, "Given ");
#line hidden
#line 74
 testRunner.When("\'Folder4\' folder is moved to \'Folder6\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                            "Id",
                            "Name",
                            "Type",
                            "ParentId",
                            "HierarchyPath"});
                table6.AddRow(new string[] {
                            "1",
                            "File1",
                            "File",
                            "",
                            ""});
                table6.AddRow(new string[] {
                            "2",
                            "Folder1",
                            "Folder",
                            "",
                            ""});
                table6.AddRow(new string[] {
                            "3",
                            ". File2",
                            "File",
                            "2",
                            "/2/"});
                table6.AddRow(new string[] {
                            "4",
                            ". Folder2",
                            "Folder",
                            "2",
                            "/2/"});
                table6.AddRow(new string[] {
                            "5",
                            ".. File3",
                            "File",
                            "4",
                            "/2/4/"});
                table6.AddRow(new string[] {
                            "6",
                            ". Folder3",
                            "Folder",
                            "2",
                            "/2/"});
                table6.AddRow(new string[] {
                            "11",
                            ". File6",
                            "File",
                            "2",
                            "/2/"});
                table6.AddRow(new string[] {
                            "12",
                            ". Folder6",
                            "Folder",
                            "2",
                            "/2/"});
                table6.AddRow(new string[] {
                            "7",
                            ".. Folder4",
                            "Folder",
                            "12",
                            "/2/12/"});
                table6.AddRow(new string[] {
                            "8",
                            "... File4",
                            "File",
                            "7",
                            "/2/12/7/"});
                table6.AddRow(new string[] {
                            "9",
                            "... Folder5",
                            "Folder",
                            "7",
                            "/2/12/7/"});
                table6.AddRow(new string[] {
                            "10",
                            ".... File5",
                            "File",
                            "9",
                            "/2/12/7/9/"});
#line 75
 testRunner.Then("the result should be", ((string)(null)), table6, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
