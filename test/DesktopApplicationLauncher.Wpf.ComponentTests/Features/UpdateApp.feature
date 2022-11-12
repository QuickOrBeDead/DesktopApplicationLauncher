Feature: UpdateApp

Update current app scenarios

Scenario: When app name is updated, the parent id should be preserved
	Given the following folder structure is
		| Id | Name        | Type   | ParentId | HierarchyPath |
		| 1  | File1       | File   |		   |               |
		| 2  | Folder1     | Folder |		   |               |
		| 3  | . File2     | File   | 2        | /2/           |
		| 4  | . Folder2   | Folder | 2        | /2/           |
		| 5  | .. Folder3  | Folder | 4        | /2/4/         |
		| 6  | .. File3    | File   | 4        | /2/4/         |
		| 7  | ... Folder4 | Folder | 6        | /2/4/6/       |
		| 8  | . File4     | File   | 2        | /2/           |
		| 9  | . Folder5   | Folder | 2        | /2/		   |
	When 'File1' is renamed to 'File11'
	And 'File2' is renames to 'File21'
	And 'File3' is renames to 'File31'
	And 'File4' is renames to 'File41'
	Then the result should be
		| Id | Name         | Type   | ParentId | HierarchyPath |
		| 1  | File11	    | File   |		    | 				|
		| 2  | Folder1		| Folder |		    | 				|
		| 3  | . File21		| File   | 2        | /2/			|
		| 8  | . File41		| File   | 2        | /2/			|
		| 9  | . Folder5	| Folder | 2        | /2/			|
		| 4  | .. Folder2	| Folder | 9        | /2/9/		    |
		| 5  | ... Folder3	| Folder | 4        | /2/9/4/  	    |
		| 6  | ... File31	| File   | 4        | /2/9/4/  	    |
		| 7  | .... Folder4	| Folder | 6        | /2/9/4/6/	    |
