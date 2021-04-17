Feature: MoveFolder

Scenario: Move folder to Upper folder
	Given the following folder structure is
		| Id | Name         | Type   | HierarchyPath |
		| 1  | File1	    | File   |				 |
		| 2  | Folder1		| Folder |				 |
		| 3  | . File2		| File   | /2/			 |
		| 4  | . Folder2	| Folder | /2/			 |
		| 5  | .. Folder3	| Folder | /2/4/		 |
		| 6  | .. File3		| File   | /2/4/		 |
		| 7  | ... Folder4	| Folder | /2/4/6/		 |
		| 8  | . File4		| File   | /2/			 |
		| 9  | . Folder5	| Folder | /2/			 |
	When 'Folder2' folder is moved to 'Folder5'
	Then the result should be
		| Id | Name         | Type   | HierarchyPath |
		| 1  | File1	    | File   | 				 |
		| 2  | Folder1		| Folder | 				 |
		| 3  | . File2		| File   | /2/			 |
		| 8  | . File4		| File   | /2/			 |
		| 9  | . Folder5	| Folder | /2/			 |
		| 4  | .. Folder2	| Folder | /2/9/		 |
		| 5  | ... Folder3	| Folder | /2/9/4/  	 |
		| 6  | ... File3	| File   | /2/9/4/  	 |
		| 7  | .... Folder4	| Folder | /2/9/4/6/	 |