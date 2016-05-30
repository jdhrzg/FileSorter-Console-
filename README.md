# FileSorter-Console-  

FileSorter is a simple command line tool used to organize your mess of files into nicely sortable by date folders.  

Example:  

*Original - out of order, hard to find what you are looking for*  
Directory "C:\Users\Joe\Pictures"  
--Name----------Date------  
img001.jpg		9/13/2016  
img002.jpg		10/10/2015  
img003.jpg		10/1/2015  
img004.jpg		7/25/2014  
img005.jpg		9/11/2014  

*Run FileSorter - group by month, folders created when needed to hold date sorted files*  

Directory "C:\Users\Joe\Pictures"  
--Name----------Date------  
2014-7  
	img004.jpg		7/25/2014  
2014-9  
	img005.jpg		9/11/2014  
2015-10  
	img003.jpg		10/1/2015  
	img002.jpg		10/10/2015  
2016-9  
	img001.jpg		9/13/2016  
