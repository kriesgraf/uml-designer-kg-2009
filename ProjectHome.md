[![](http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/Rss.png)](http://code.google.com/feeds/p/uml-designer-kg-2009/downloads/basics) RSS feeds: [New downloads](http://code.google.com/feeds/p/uml-designer-kg-2009/downloads/basics) -  [New issues](http://code.google.com/feeds/p/uml-designer-kg-2009/issueupdates/basic) - [Others](http://code.google.com/p/uml-designer-kg-2009/feeds)

The new Release version 1.0.3.3 kit is already available [here](http://uml-designer-kg-2009.googlecode.com/files/ClassXmlProject_1_0_3_3.exe).

# Table of contents #

## [User manual](Getting_started.md) ##
  * [Getting started](Getting_started.md)
  * [Troubleshooting ](Troubleshooting.md)

## [Software design](Software_design.md) ##
  * [Package overview](Software_design.md)
  * [UML Project data file structure](Software_design_2.md)

# Introduction #

## But what does it ? ##

You only have 10 minutes to make a class diagram and generate C++ code corresponding to a model of your preliminary project, this application will help you.

You can declare packages and the classes they contain, declare relations between classes (associations/compositions/aggregations), declare based/predefined types and enumerations, declare dependencies with external components of a framework or a library.

The display is customized according to the target language (C ++, VB.NET for now), but the [GUI](http://en.wikipedia.org/wiki/GUI) allows a more general syntax that considers more other object-oriented languages like Java, Python, etc.

You can declare some classes as import from other projects or libraries in a simple way, whether JAVA or Python packages, .NET namespaces, header files of a C++ Windows DLL & Linux SO files, etc ...

You can do reverse engineering using XML [Doxygen](http://www.stack.nl/~dimitri/doxygen/) export or import an XML file using [UML2.1](http://www.omg.org/spec/UML/2.1.2/) format of http://www.omg.org.

When you have finished design your project, or you want to check at one step the generated code, you can display it without writing any file. When you finally want to generate code, choose a repository and execute command.

Sorry but now, the last content could be overwritten:
  * In C++, only header file H is created/overwritten, but you can edit inline method body inside application. Contrary, the source file CPP should be done by yourself.
  * In VB.NET, only one source file that contains header & body is created, but at next update, you can choose an external tool like [WinMerge](http://winmerge.org/) or a Vb code inner tool. Generated code presentation is optimized to be merged easy, also don't move blocks inside modules.

## For end user ##

First download the current [release](http://code.google.com/p/uml-designer-kg-2009/downloads/list) and execute archive extractor upon file receipt. Save it in a specific folder as it's proposed: `C:\Program files\ClassXmlProject`.

Remain it because you would have an error when you will install future releases, see [troubleshooting](Troubleshooting.md).

Program `setup.exe` is launched automatically and installs all files, resources and frameworks that are required. If the setup fails to install the **Microsoft .NET 3.5** framework, you can download framework [here](http://download.microsoft.com/download/0/6/1/061F001C-8752-4600-A198-53214C69B51F/dotnetfx35setup.exe).

If all goes well, the application starts automatically. Also, if you're one of the happy users who have succeed to start the program, I continue to speak about software manual [here](Getting_started.md).

## For developers ##

Software development choices:
  * [XML](http://www.w3.org/XML/) format is the choice to store project data,
  * [VB Express 2008](http://www.microsoft.com/Express/VB/) & framework [.NET 3.5](http://download.microsoft.com/download/0/6/1/061F001C-8752-4600-A198-53214C69B51F/dotnetfx35setup.exe) is used to create the GUI, in fact only DLL from framework 2.0 are used.
  * [XSL](http://www.w3.org/Style/XSL/) is the language to convert XML project data files into HTML display and generated code source.

All developed classes use or are based on the keystones:
  * The Design Pattern [(ToBeDefined)](http://www.dofactory.com/Patterns/Patterns.aspx), **XmlComponent** in this project, assumes data exchange with [DOM objects](http://www.w3.org/DOM/) of namespace [System.Xml](http://msdn.microsoft.com/en-us/library/system.xml.aspx) (element nodes and attribute nodes).
  * The Design Pattern [Composite](http://www.dofactory.com/Patterns/PatternComposite.aspx), **XmlComposite** in this project, provides a second coverage of the DOM tree pointing only business objects. Inherits **XmlComponent**.
  * The Design Pattern [Prototype](http://www.dofactory.com/Patterns/PatternPrototype.aspx), **XmlNodeManager** in this project, has the burden to create objects needed for data display/update managements.

Each project data display is made of 3 classes:
  * _Dialog_ class : display project data in Windows forms. Inherits the class [System.Windows.Forms.Form](http://msdn.microsoft.com/en-us/library/system.windows.forms.form.aspx)
  * _Document_ class : allows data exchange with DOM objects from System.Xml. Inherits **XmlComponent** or customize an another _Document_ class.
  * _View_ class : allows data exchange with controls ([System.Windows.Forms.Control](http://msdn.microsoft.com/en-us/library/system.windows.forms.control.aspx)) which owns the _Dialog_ class. Inherits a _Document_ class or customize an another _View_ class.

Want to know more about the software design, follow the [link](Software_design.md).
