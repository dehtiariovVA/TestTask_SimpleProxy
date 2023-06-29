# Test task description

-	Write a "proxy" that will return the modified text of any site of your choice;
-	To each word, which consists of six letters, you must add a symbol "™";
-	You can use any .Net technology and libraries in the C# language;
-	The functionality of the original site must not be altered;
-	All internal navigation links of the site must be replaced by the address of the proxy-server.

That is, site navigation must be handled by a Proxy without taking the client back to the original site.

Example. A request to, say, {proxy address}/en-us/docs/ should show the content of the page https://learn.microsoft.com/en-us/docs/ with changed words that were 6 characters long. 
And all the site navigation to sections of the site should go through Proxy.
