<h2>Content:</h2>


# Installation #

First download the current [release](http://code.google.com/p/uml-designer-kg-2009/downloads/list) and execute archive extractor upon file receipt. Save it in a specific folder as it's proposed: `C:\Program files\ClassXmlProject`.

Remain it because you would have an error when you will install future releases, see [troubleshooting](Troubleshooting.md).

Program `setup.exe` is launched automatically and installs all files, resources and frameworks that are required. If the setup fails to install the **Microsoft .NET 3.5** framework, you can download framework [here](http://download.microsoft.com/download/0/6/1/061F001C-8752-4600-A198-53214C69B51F/dotnetfx35setup.exe).

The old releases remain on disk as long as you don't _remove the application from_ your _computer_. Also you can _restore the application to its previous state_ by descending order history.

# Quick tips #

## Pop-up-menu ##

Most commands can be accessed by right click on the mouse.

Usually an icon is associated with each menu item to find it faster. Moreover, the objects and commands that can create them have the same icon.

A Menu item unavailable for an object is not yet disable but cause no error.

## Edition commands ##

The Edit command is always reachable with a mouse double-click, except in project list view:
  * An object without child elements: opens the associated editor window.
  * An object with children: opens a sub-level in project list view.

Grid view cell and list view label edition is always reachable with a long time mouse click.

Sometimes, editing grid view cells with mouse click is disabled, recommend to use F2 key or press alphanumeric character keys.

Only Copy/Cut/Paste commands are available in menu bar, all others are found in pop-up-menus.

## Drag and drop ##

The drag and drop command is available to reorder objects in list boxes and data grid views. But this command is not available when the order is unnecessary.

This command is available in C++ language project for **typedef** declarations. To drag object: press key _Control_ and click left in typedef _Name_ cell, then hold down mouse and _Control_ key and drag object to its new location an drop it.

This is available too in list views without pressing _Control_ key:
  * To move objects down below into a sub-package,
  * To merge imports that have the same full path name, including empty field.

## Working directory ##

For now, the application does not manage neither project workspaces, nor project files' history. But, maintains a list of folders that you currently use.

# UML concepts #

To make class/collaborations/package diagrams and generate VB.NET code in 10 minutes, we have to reduce UML modelling to **class**, **package** elements and **relationship**.

The element **import** takes the concepts of component/interface. But we must think as Java imports, VB namespaces or C++ libraries (DLL/SO).

## Package element ##

This element is a grouping of model elements. Packages themselves may be nested within other packages. A package may contain both subordinate packages and ordinary model elements like **class**.

This can contain **import** elements to improve readability.

## Class element ##

This element is the descriptor for a set of objects with similar structure, behavior, and relationships.

The parameters of the class are:
  * **Constructor**: _true/false_, default constructor implemented or not.
  * **Destructor**: _true/false_ (only used in C++ and VB.NET)
  * **Package visibility**: _package_ (full visibility) / _common_ (current package visibility).
  * **Implementation**: _simple_, _leaf_, _node_, _root_, this four values describes position of the class inside inheritance tree, _interface_, a pure abstract class, _exception_, a simple class identified in exception handling, _container_, a class with one or more unbound formal parameters that we call usually _template_.

This is a grouping of sub-model elements:
  * **Property**: this element describes the UML element **attribute** and its own **operations** that we call usually _accessors_ or _getter/setter_.
  * **Method**: this element describes a general UML element **operation**.
  * **Constructor**: this element describes a specific **operation** that instantiates this class (Note: the default constructor like destructor are class parameters).
  * **Typedef**, **Enumeration**, **Container** and **Structure** are UML **nested classes** that define _datatypes_.

To be continue....

## Relationship element ##

To be continue....

## Import element ##

To be continue....