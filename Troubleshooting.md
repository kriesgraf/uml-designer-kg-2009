<h2>Content:</h2>


# Troubleshooting #

Some warning messages appear when you save/copy/open a project in a different/new folder or when you install a new application release. The primary cause is the file 'class-model.dtd'. At each new release installation, original file 'class-model.dtd' in application resources is created and also its file creation date differs from one in your projects' workspace folder.

The DTD version (current is 1.2) is not yet verified. This warning message is sometimes improper but it's better to know now exactly the consequence of copying this file. In a future release, the DTD version will be verified but the following warning messages will remain.

Other warning messages appear during installation or execution of application, these messages are detailed below and a good or alternative solution is proposed.

Some errors can be unrecoverable, an **Exception** message is also displayed. This message confirms that current action has failed and stopped, but the current edited project is not lost. Sometimes, you could continue edit this and save later without any trouble.

## Cannot start application ##

![http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Cannot%20start%20application%20from%20this%20location.png](http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Cannot%20start%20application%20from%20this%20location.png)

This message appears when you have not follow the installation [guidance](Getting_started.md). Two solutions may be proposed:
  * Inflate last release archive in the same folder as the previous one.
  * Open the **Control panel** and the launch tool "**Add or remove programs**". The select ClassXmlProject application and uninstall it. Then a message box proposes to you: 1) recover previous release, 2) remove all releases, choose last one, please.

## Can't find File '... ...\class-model.dtd' ##

![http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Class%20model%20conflict%20-%20local%20project%20DTD%20does%20not%20exist.png](http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Class%20model%20conflict%20-%20local%20project%20DTD%20does%20not%20exist.png)

The project you try to open has been moved from its previous workspace folder or its associated file 'class-model.dtd' has been moved/renamed/deleted by mistake.

if you confirm the question, a new file 'class-model.dtd' will be created in this folder.

## File '... ...\class-model.dtd' is more older than application resource ##

![http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Class%20model%20conflict%20-%20local%20project%20DTD%20older%20than%20resource.png](http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Class%20model%20conflict%20-%20local%20project%20DTD%20older%20than%20resource.png)

You have installed a new release and the original file 'class-model.dtd' in application resources is also more recent.

We suggest you to save your current project in a different folder. But if you confirm the question and tick the check box, an original file 'class-model.dtd' will be copied in your project workspace folder.

## File '... ...\class-model.dtd' is more recent than application resource ##

![http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Class%20model%20conflict%20-%20more%20recent%20than%20resource.png](http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Class%20model%20conflict%20-%20more%20recent%20than%20resource.png)

![http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Class%20model%20conflict%20-%20more%20recent%20than%20resource2.png](http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Class%20model%20conflict%20-%20more%20recent%20than%20resource2.png)

You have installed or recover an older release and the original file 'class-model.dtd' in application resources is also older.

We suggest you not to confirm question. But if you want do it, an original file 'class-model.dtd' will be copied in your project workspace folder.

## Exception messages ##

The exceptions are raised by program, while:
  * some mathematical operations are incorrect,
  * some object references are lost,
  * the use of framework component is incorrect,
  * a file operation is forbidden and was not controlled preventively: file does not exist, file cannot be overwritten, current user has no right to read/write in the target folder,
  * etc.

This message is displayed with a technical description and a _call stack_: an history of all methods called before raising this exception .

Click on the big icon and you will have an history of methods called in **.NET** framework, but only if this exception is raised inside.

Click on the technical description and all details of error is copied to clipboard.

http://uml-designer-kg-2009.googlecode.com/svn/trunk/User%20manual/ClassXmlProject/Exception%20-%20Project%20incompatible%20with%20DTD.PNG

This image describes the most common exception, or the only one that could have occurred. You overwrote the DTD file in you project workspace, but the project that you attempt to open is to old and has not been upgraded.

Another case, even more rare that the DTD has not been changed, you saved your project without problems but when re-open it, you had this error message. And then, nothing to do well by taking the old DTD. The DTD is not involved, the application has made an error of inserting a node in the XML tree. Soon you will have a patch to recover your project. Waiting for this patch, you could use a XML Validator to locate the error and correct it yourself.

Apart from this characteristic error, the other should be more episodic and caused by a particular configuration of your computer or a particular structure of your project.

Also we do not detail but we will show you how to proceed:
  * Read the message carefully and calmly, but do not press the OK button frantically!
  * If you have understand why this error occurs, press OK and retry you last operation carefully, fill out the forms with the correct values, choose the correct file, the correct folder and go.
  * If you do not understand or the error remains, remember your last four actions, you write these actions on a scrap paper.
  * In message box, click first on the technical description to copy details to clipboard, then click on link [Send a new issue](http://code.google.com/p/uml-designer-kg-2009/issues/entry).
  * MS Internet Explorer should display the **New issue** [web page](http://code.google.com/p/uml-designer-kg-2009/issues/entry).
  * Paste clipboard at the end of the **Description** text box.
  * You can no enter the four last actions below the question _What steps will reproduce the problem_.
  * You can attach a project file that can reproduce this exception.
  * Press **Submit issue**.