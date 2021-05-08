Feature: MoveFolder

Scenario: Move folder to Same-level folder
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
	When 'Folder2' folder is moved to 'Folder5'
	Then the result should be
		| Id | Name         | Type   | ParentId | HierarchyPath |
		| 1  | File1	    | File   |		    | 				|
		| 2  | Folder1		| Folder |		    | 				|
		| 3  | . File2		| File   | 2        | /2/			|
		| 8  | . File4		| File   | 2        | /2/			|
		| 9  | . Folder5	| Folder | 2        | /2/			|
		| 4  | .. Folder2	| Folder | 9        | /2/9/		    |
		| 5  | ... Folder3	| Folder | 4        | /2/9/4/  	    |
		| 6  | ... File3	| File   | 4        | /2/9/4/  	    |
		| 7  | .... Folder4	| Folder | 6        | /2/9/4/6/	    |

Scenario: Move folder to Lower-level folder
	Given the following folder structure is
		| Id | Name        | Type   | ParentId | HierarchyPath |
		| 1  | File1       | File   |          |               |
		| 2  | Folder1     | Folder |          |               |
		| 3  | . File2     | File   | 2        | /2/           |
		| 4  | . Folder2   | Folder | 2        | /2/           |
		| 5  | .. File3    | File   | 4        | /2/4/         |
		| 6  | . Folder3   | Folder | 2        | /2/           |
		| 7  | .. Folder4  | Folder | 6        | /2/6/         |
		| 8  | .. File4    | File   | 6        | /2/6/         |
		| 9  | ... Folder5 | Folder | 8        | /2/6/8/       |
		| 10 | .... File5  | File   | 9        | /2/6/8/9/     |
		| 11 | . File6     | File   | 2        | /2/           |
		| 12 | . Folder6   | Folder | 2        | /2/		   |
	When 'Folder2' folder is moved to 'Folder5'
	Then the result should be
		| Id | Name         | Type   | ParentId | HierarchyPath |
		| 1  | File1	    | File   |          |				|
		| 2  | Folder1		| Folder |          |				|
		| 3  | . File2		| File   | 2        | /2/			|
		| 6  | . Folder3	| Folder | 2        | /2/			|
		| 7  | .. Folder4	| Folder | 6        | /2/6/		    |
		| 8  | .. File4		| File   | 6        | /2/6/		    |
		| 9  | ... Folder5	| Folder | 8        | /2/6/8/		|
		| 10 | .... File5	| File   | 9        | /2/6/8/9/	    |
		| 4  | .... Folder2	| Folder | 9        | /2/6/8/9/	    |
		| 5  | ..... File3  | File   | 4        | /2/6/8/9/4/	|
		| 11 | . File6		| File   | 2        | /2/			|
		| 12 | . Folder6	| Folder | 2        | /2/			|

Scenario: Move folder to Upper-level folder
	Given the following folder structure is
		| Id | Name         | Type   | ParentId | HierarchyPath |
		| 1  | File1	    | File   |          |				|
		| 2  | Folder1		| Folder |          |				|
		| 3  | . File2		| File   | 2        | /2/			|
		| 4  | . Folder2	| Folder | 2        | /2/			|
		| 5  | .. File3  	| File   | 4        | /2/4/		    |
		| 6  | . Folder3	| Folder | 2        | /2/			|
		| 7  | .. Folder4	| Folder | 6        | /2/6/		    |
		| 8  | ... File4	| File   | 7        | /2/6/7/	    |
		| 9  | ... Folder5	| Folder | 7        | /2/6/7/   	|
		| 10 | .... File5	| File   | 9        | /2/6/7/9/     |
		| 11 | . File6		| File   | 2        | /2/			|
		| 12 | . Folder6	| Folder | 2        | /2/			|
	When 'Folder4' folder is moved to 'Folder6'
	Then the result should be
		| Id | Name         | Type   | ParentId | HierarchyPath |
		| 1  | File1	    | File   |          |				|
		| 2  | Folder1		| Folder |          |				|
		| 3  | . File2		| File   | 2        | /2/			|
		| 4  | . Folder2	| Folder | 2        | /2/			|
		| 5  | .. File3  	| File   | 4        | /2/4/		    |
		| 6  | . Folder3	| Folder | 2        | /2/			|
		| 11 | . File6		| File   | 2        | /2/			|
		| 12 | . Folder6	| Folder | 2        | /2/			|
		| 7  | .. Folder4	| Folder | 12       | /2/12/		|
		| 8  | ... File4	| File   | 7        | /2/12/7/		|
		| 9  | ... Folder5	| Folder | 7        | /2/12/7/  	|
		| 10 | .... File5	| File   | 9        | /2/12/7/9/	|