Create an application that identifies the pair of employees who have worked
together on common projects for the longest period of time.
Input data:
A CSV file with data in the following format:

Sample data:
| EmpID | ProjectID | DateFrom   | DateTo     |
|-------|-----------|------------|------------|
| 349   | 2         | 2004-02-27 | 2004-02-28 |
| 226   | 2         | 2003-07-01 | NULL       |
| 310   | 2         | 2002-12-31 | NULL       |
| 445   | 3         | 2007-11-19 | NULL       |

Sample output:
| EmpIDFirst | EmpIDSecond | ProjectID | DaysWorkedTogether |
|------------|-------------|-----------|---------------------|
| 226        | 310         | 2         | 7517                |
| 310        | 349         | 2         | 1                   |
| 226        | 349         | 2         | 1                   |


Specific requirements
1) DateTo can be NULL, equivalent to today
2) The input data must be loaded to the program from a CSV file

Bonus points
1) Create an UI:
The user picks up a file from the file system and, after selecting it, all common
projects of the pair are displayed in datagrid with the following columns:
Employee ID #1, Employee ID #2, Project ID, Days worked
2) More than one date format to be supported, extra points will be given if all date formats
are supported
