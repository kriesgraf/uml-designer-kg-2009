<h2>Content:</h2>


# UML project data file structure #

We have choose [XML](http://www.w3.org/TR/xml/) format for project data file to allow a loads of possible conversion, at least HTML display and source code generation.

For now the structure of the Xml document is declared in a Document Type Definition file ([DTD](http://www.w3.org/TR/xml/#sec-prolog-dtd)), which current version is [1.3](http://uml-designer-kg-2009.googlecode.com/svn/tags/ClassXmlProject_1_0_2_3/xslt/class-model.xml).

## Overview ##

Xml format offers a tree data structure that is useful to describe Uml project description.

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/UML%20project%20data%20file%20main%20structure.PNG
  * First **root** node is the base of the Xml tree, attributes declare info about project file.
  * Attribute **name**: is the name of the project, but this could be used as _root namespace_ and also should observe language code syntax.
  * Attribute **version**: is fixed at project file creation, but could be upgraded when a _patch_ is apply.
  * First children nodes **generation** and **comment** are associated to node **root** and describe code generation _language_ and _repository_, and the last more detailed info about project.
  * Following nodes **import**, **class**, **package** and **relationship** can have a cardinal of 0..n according to the project description. Each nodes is detailed in a specific paragraph.

## Xml "import" node structure ##

To simplify the **import** node describes both an external component that contains **references** and **interfaces** (in future version).

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/UML%20project%20data%20file%20import%20node%20structure.PNG

  * Attribute **visibility** allows to share declaration between all packages or not, values: _package/common_.
  * **param** could remain empty when attribute **name** is comprehensive. This is generally a _full path_ declaration that is inserted in source code. See example in _code generation_ chapter in this manual.
  * Node **reference** describes a _class_ or a _nested-class_ of this component.
  * Node **interface** (in future version) describes an _abstract class_ that can be use to interface application with this component.

## Xml "class" node structure ##

To simplify the **class** node describes both the module that will be generated and the element of the project design. Some language allow to declare several types and classes in a same module, we have decided to choose the Java declaration: one _class = one module_.

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/UML%20project%20data%20file%20class%20node%20structure.PNG

  * Attributes **constructor** and **destructor** are declared according to the code generation language and user choice.
  * Attribute **implementation** declares this class as an _interface_, a _simple_ and independant class, or is the _root_, _node_ or _leaf_ of a huge inheritance tree.
  * Node **comment** is used to declare comment for Doxygen, Visual Studio comment tools or javadoc (or other in future).
  * Following nodes **inherited**, **dependency**, **collaboration**, **import**, **typedef**, **property** and **method** can have a cardinal of 0..n according to the project description. Each nodes is detailed in a specific paragraph.

## Xml "package" node structure ##

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/UML%20project%20data%20file%20package%20node%20structure.PNG

  * Attribute **folder** could remain empty when attribute **name** is comprehensive. This is generally used to declare a _relative path_ from project _repository_ (see **generation** node) where class source code will be generated.
  * Node **comment** is used to declare comment for Doxygen, Visual Studio comment tools or javadoc (or other in future).
  * Following nodes **import**, **class**, **package** can have a cardinal of 0..n according to the project description. Each nodes is detailed in a specific paragraph.

## Xml "property" node structure ##

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/UML%20project%20data%20file%20property%20node%20structure.PNG

  * Attribute **member** declares the property, member of _class_ (static/shared variable) or _object_ (simple variable).
  * Nodes **type** and **variable** declare _datatype_ and are described further.
  * Node **comment** is used to declare comment for Doxygen, Visual Studio comment tools or javadoc (or other in future).
  * Node **get** declares _getter_ and its visibility: _range_ (public/protected), its return value type: _modifier_ (const/var), _by_ (reference, value).
  * Node **set** declares _setter_ and its visibility: _range_ (public/protected), its argument value type: _by_ (reference, value).

We provided 3 more optional attributes:
  * **behaviour**: used at least in Visual Basic,
  * **overridable**: _yes/no_, future use to allow overriding accessors in a derived class.
  * **overrides**: **id** attribute of the inherited **class** node or implemented **interface** node.

## Xml "method" node structure ##

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/UML%20project%20data%20file%20method%20node%20structure.PNG

  * Attribute **member** declares the method, member of _class_ (static/shared variable) or _object_ (simple variable).
  * Attribute **implementation** declares this method as _abstract_, _overridable_, _overrides_ or _final_ in a huge inheritance tree, or as _simple_ method.
  * Attribute **constructor** declares if the method is special constructor, thsi attribute declares the range of this method: _no/private/protected/public_.
  * Nodes **type** and **variable** declare _datatype_ and are described further.
  * Node **exception** has a a cardinal of 0..n corresponding to the number of exceptions thrown by the method. Its attribute **idref** links a node **class** or a **reference**. This declaration is not used in Visual Basic language.
  * Node **return** is defined when method is not a constructor, this node has the same structure as node **property**, **typedef** and **param** and declare its _datatype_.
  * Node **comment** is used to declare comment for Doxygen, Visual Studio comment tools or javadoc (or other in future).
  * Node **param** can have a cardinal of 0..n according to the method declaration, this is an argument of this method. Sub-level node **type** and **variable** declares its _datatype_. Attribute **value** in node **variable** declares default value when argument is provided in call of this method.

We provided 3 more optional attributes:
  * **behaviour**: used at least in Visual Basic,
  * **operator**: declares a special method called _operator_.
  * **overrides**: **id** attribute of the inherited **class** node or implemented **interface** node.


To be continue...