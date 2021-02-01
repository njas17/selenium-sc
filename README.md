## SeleniumSitecoreTest

### Information / Details

#### Using

.Net framework 4.6.1

IDE Visual Studio 2017

#### Dependencies 
----------------------------------------------------------

* Ensure you create a path in C drive. I.e -'C:\chromedriver' and you should have the chromedriver.exe (chrome driver version is 88.0.4324.06 (windows)) in the folder. If you don't have the driver, try to get the driver here - http://chromedriver.storage.googleapis.com/index.html?path=88.0.4324.96/) or google for it.

* Packages (installed from NuGet Package Manager)

	1. Selenium.WebDriver v3.141.0
	2. Selenium.Support v3.141.0
	3. MSTest.TestFramework v2.1.2
	
#### Testcase for the following steps:
1) Navigate to "https://gofile.io/d/9YsNjv"

2) Get the value of the element "code.language-txt"

3) Navigate to http://www.unit-conversion.info/texttools/hexadecimal/

4) On the Convert dropdown,select the "hex numbers to text" option

5) In the input data text area, input the data from step 2

6) Copy the output and save it into a text file (Using Code)

	*Location of the text file must be inside "C:\InterviewSitecore"
	
	*The Folder & text file must be created new (Using Code)
	
-Assert that files in C:\InterviewSitecore is present.

New Testcase:

1) Change the Folder name from InterviewSitecore to SitecoreTest.

2)Read the file in C:/InterviewSitecore .

3) Navigate to "https://gofile.io/d/E77oZ4" and input the password

	*The password must be read from textfile in C:/InterviewSitecore
	
4) Click OK	

5) Download the image then store it in C:/InterviewSitecore

-Assert Folder name is change.

-Assert image is there
