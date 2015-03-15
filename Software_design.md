<h2>Content:</h2>


# Packages overview #

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/UML%20Package%20diagram.PNG

I will use the term _namespace_ instead of package because this is the VB syntax term and this avoids confusion with Xml tag [nodes](Software_design_2.md) that describes UML projects.

## Namespace xml ##

The keystones are defined in namespace **xml**. Look at sources in SVN repository
in folder  [svn/trunk/ClassXmlProject/xml/](http://code.google.com/p/uml-designer-kg-2009/source/browse/#svn/trunk/ClassXmlProject/xml), in class declaration, namespace is not yet declared! It's not a mistake, this application is still experimental!
  * The Design Pattern [(ToBeDefined)](http://www.dofactory.com/Patterns/Patterns.aspx), <b>XmlComponent</b> in this project, assumes data exchange with [DOM objects](http://www.w3.org/DOM/) of namespace [System.Xml](http://msdn.microsoft.com/en-us/library/system.xml.aspx) (element nodes and attribute nodes).
  * The Design Pattern [Composite](http://www.dofactory.com/Patterns/PatternComposite.aspx), <b>XmlComposite</b> in this project, provides a second coverage of the DOM tree pointing only business objects. Inherits <b>XmlComponent</b>.
  * The Design Pattern [Prototype](http://www.dofactory.com/Patterns/PatternPrototype.aspx), <b>XmlNodeManager</b> in this project, has the burden to create objects needed for data display/update managements.


Other classes are declared in this namespace and assume some interactions with DOM elements:
  * **XslSimpleTransform** encapsulates class [XslCompiledTransform](http://msdn.microsoft.com/en-us/library/system.xml.xsl.xslcompiledtransform.aspx) from namespace [System.Xml.Xsl](http://msdn.microsoft.com/en-us/library/system.xml.xsl.aspx) to provide new interface methods.
  * **XmlReferenceNodeCounter** and **XmlNodeCounter** generate unique ID for XML attributes typed [ID](http://www.w3.org/TR/REC-xml/#id) and [IDREF](http://www.w3.org/TR/REC-xml/#idref).

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/Xml%20package%20class%20diagram.PNG

## Namespace documents ##

As we saw in the introduction, the classes of this namespace have the same ancestor **XmlComponent**. Look at sources in SVN repository
in folder  [svn/trunk/ClassXmlProject/documents/](http://code.google.com/p/uml-designer-kg-2009/source/browse/#svn/trunk/ClassXmlProject/documents)

The aim of this namespace is to provide in interface some business classes which expose only properties and methods. The interface with DOM objects is hidden (protected methods).

Each class of this namespace has the burden to build DOM objects: which tag name, which attributes, which children, these impose the [structure (DTD)](Software_design_2.md) of the XML project data document.

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/Documents%20package%20class%20diagram.PNG

## Namespace bindings ##

As we will often mention these classes in the following class diagrams, it was preferable to present them now. Look at sources in SVN repository
in folder  [svn/trunk/ClassXmlProject/bindings/](http://code.google.com/p/uml-designer-kg-2009/source/browse/#svn/trunk/ClassXmlProject/bindings)

  * The class **XmlBindingList** manages a pool of [Binding](http://msdn.microsoft.com/en-us/library/system.windows.forms.binding.aspx) objects from namespace [System.Windows.Forms](http://msdn.microsoft.com/en-us/library/system.windows.forms.aspx). Each one binds a property of a **namespace view** class with a property of a **Control** object, example: property _Text_ for a [TextBox](http://msdn.microsoft.com/en-us/library/system.windows.forms.textbox.aspx).

  * The class **XmlBindingDataGridView** manages data bindings between the children of a **XmlComposite** object and a [DataGridView](http://msdn.microsoft.com/en-us/library/system.windows.forms.datagridview.aspx). <br />It was necessary to design this class to maximize usage of polymorphic calls to set special DataGridView properties, to fill columns and rows (see interface [InterfViewControl](http://code.google.com/p/uml-designer-kg-2009/source/browse/trunk/ClassXmlProject/xml/XmlComponent.vb)). And also to answer to Windows events: click, delete, add, drag, etc (See interface [InterfGridViewNotifier](http://code.google.com/p/uml-designer-kg-2009/source/browse/trunk/ClassXmlProject/bindings/XmlBindingDataGridView.vb)).

  * The class **XmlBindingDataListView** manages data bindings between the children of a **XmlComposite** object and a **DataListView**. <br />It was necessary to design **XmlBindingDataListView** to maximize usage of polymorphic calls to set special [ListView](http://msdn.microsoft.com/en-us/library/system.windows.forms.listview.aspx) properties, to draw objects (see interface [InterfViewControl](http://code.google.com/p/uml-designer-kg-2009/source/browse/trunk/ClassXmlProject/xml/XmlComponent.vb)). And also to answer to Windows events: click, delete, add, drag, etc (See interface [InterfListViewNotifier](http://code.google.com/p/uml-designer-kg-2009/source/browse/trunk/ClassXmlProject/bindings/XmlBindingDataListView.vb)). <br />The **XmlBindingDataListView** provides a hierachical view of a UML project, a double mouse click allows to explore sub-levels, a drag & drop move an object to a sub-level. The relation _Stack_ allows to save history and go back from sub-levels and relation _ParentNode_ save current level.

  * The class **XmlBindingCombo** manages only one combo. It was necessary to design this class, similar as a [Binding](http://msdn.microsoft.com/en-us/library/system.windows.forms.binding.aspx), because [ComboBox](http://msdn.microsoft.com/en-us/library/system.windows.forms.combobox.aspx) can display a list of data or can be used as a simple input field like a **TextBox**.

  * As against, it was not necessary to specialise class [ListBox](http://msdn.microsoft.com/en-us/library/system.windows.forms.listbox.aspx) because this already owns a _DataSource_ property. For example, we can build an [ArrayList](http://msdn.microsoft.com/en-us/library/system.collections.arraylist.aspx) of **XmlComponent** objects and use it as DataSource.

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/Bindings%20package%20classes%20diagram.PNG

## Namespace controls ##

As we will often mention these classes in the following class diagrams, it was preferable to present them now. Look at sources in SVN repository
in folder  [svn/trunk/ClassXmlProject/controls/](http://code.google.com/p/uml-designer-kg-2009/source/browse/#svn/trunk/ClassXmlProject/controls)

  * The **DataListView** is not a class from namespace [System.Windows.Forms](http://msdn.microsoft.com/en-us/library/system.windows.forms.aspx), we decided to customize class  [ListView](http://msdn.microsoft.com/en-us/library/system.windows.forms.listview.aspx) to have the same level of collaboration than a [DataGridView](http://msdn.microsoft.com/en-us/library/system.windows.forms.datagridview.aspx) class (data binding management).

  * The **XmlDataListView** customizes class **DataListView** to collaborate with **XmlBindingDataListView**.

  * The **XmlDataGridView** customizes class **DataGridView** to collaborate with **XmlBindingDataGridView**.

  * The **XmlDocumentView** customizes [WebBrowser](http://msdn.microsoft.com/en-us/library/system.windows.forms.webbrowser.aspx) class to provide several displays converting Xml format project data file (relation _currentNode_) with XSL transformations, processed by class **XslSimpleTransform** (relation _Stylesheet_).

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/Controls%20package%20class%20diagram.PNG

## Namespace view ##

Each project data display is the collaboration of 3 classes:
  * _Dialog_ class : display project data in Windows forms. Inherits the class [Form](http://msdn.microsoft.com/en-us/library/system.windows.forms.form.aspx).
  * _Document_ class : allows data exchange with DOM objects from System.Xml. Inherits **XmlComponent** or customize an another _Document_ class.
  * _View_ class : allows data exchange with controls ([System.Windows.Forms.Control](http://msdn.microsoft.com/en-us/library/system.windows.forms.control.aspx)) which owns the _Dialog_ class. Inherits a _Document_ class or customize an another _View_ class.

Look at sources in SVN repository
in folder  [svn/trunk/ClassXmlProject/view/](http://code.google.com/p/uml-designer-kg-2009/source/browse/#svn/trunk/ClassXmlProject/view)

### Class Global View ###

The class **XmlClassGlobalView** has burden to create/update each Xml tag [class](Software_design_2.md) node of the UML project data file.

The class **dlgClass** with its own controls manage the GUI. In this window form, we can find [ComboBox](http://msdn.microsoft.com/en-us/library/system.windows.forms.combobox.aspx), [TextBox](http://msdn.microsoft.com/en-us/library/system.windows.forms.textbox.aspx) and [DataGridView](http://msdn.microsoft.com/en-us/library/system.windows.forms.datagridview.aspx) controls, this last use is detailed in next paragraph.

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/Class%20global%20view%20diagram%201-2.PNG

### Class Global View children ###

The Xml tag [class](Software_design_2.md) node of the UML project data file has several children:

  * Node [collaboration](Software_design_2.md): this references a "relation" node that declares a collaboration between to classes. **XmlClassRelationView** and is parent class **XmlCollaborationSpec** manage display in [DataGridView](http://msdn.microsoft.com/en-us/library/system.windows.forms.datagridview.aspx) and cells updates.

  * Node [dependency](Software_design_2.md): this references another <u>class</u> node that is in dependency. **XmlClassDependencyView** and is parent class **XmlDependencySpec** manage display in DataGridView and cells updates.

  * **XmlClassMemberView** manages display in DataGridView and cells updates for nodes : [typedef](Software_design_2.md), [property](Software_design_2.md) and [method](Software_design_2.md).

  * Node [inherited](Software_design_2.md): this references another <u>class</u> node that is its mother. **XmlClassInheritedView** manages display in DataGridView and cells updates for this node.

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/Class%20global%20view%20diagram%202-2.PNG

### Type and Variable View ###

To be continue...

### Property and Param View ###

In the UML design model:

  * The property is the member attribute of a class. UML description and code generation info is input in window form: **dlgProperty**.

  * The param is the argument of a class member function. UML description and code generation info is input in window form: **dlgParam**.

In the namespace **view**:

  * The class **XmlPropertyView** has burden to create/update each Xml tag [property](Software_design_2.md) node of the UML project data file.

  * The class **XmlParamView** has burden to create/update each Xml tag [param](Software_design_2.md) node of the UML project data file.

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/Property%20Param%20view%20diagram.PNG

### Method View ###

In the UML design model, The method is the member function of a class. UML description and code generation info is input in window form: **dlgMethod**.

The class **XmlMethodView** has burden to create/update each Xml tag [method](Software_design_2.md) node of the UML project data file.

As special class constructors are methods with arguments but without return value, it was easily to customize _method view_ components: **dlgConstructor**, **XmlConstructorSpec** and **XmlConstructorView**. This last creates/updates the same Xml tag [method](Software_design_2.md) node, only some Xml attributes and children nodes differ than a classical <u>method</u> node.

The Xml tag [method](Software_design_2.md) node of the UML project data file has two kinds of children:

  * Node [exception](Software_design_2.md): this references another <u>class</u> node that is used to raise and throw exceptions.

  * Node [param](Software_design_2.md): declares an argument of this method.

The **XmlMethodMemberView** class manages display in [DataGridView](http://msdn.microsoft.com/en-us/library/system.windows.forms.datagridview.aspx) and cells updates for nodes : [exception](Software_design_2.md) and [param](Software_design_2.md).

http://uml-designer-kg-2009.googlecode.com/svn/trunk/Software%20design/ClassXmlProject/Method%20Constructor%20view%20diagram.PNG

To be continue...