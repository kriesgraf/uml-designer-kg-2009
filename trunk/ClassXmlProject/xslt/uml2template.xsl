<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
  <!-- =======================================================================
  This style sheet is a template, you can customize it as you want, but becareful, don't remove
  mandatory blocks that are required by process, especially all "xsl:template" blocs with attribute 'mode': "Node"

  On the other hand, you can remove "xsl:template" blocs with attribute 'mode': "Code" or add new "xsl:template" blocs.
  Don't remove main "xsl:template" block with attribute 'match': "/root"

  If process is upgraded, an automatic correction will be applied to your style sheet a,d with upgrade
  only mandatory XSL nodes.

  Note: in some case XML generated nodes could contains the namespace declaration attribute "xmlns:msxsl".
        This is not important, except if you generate XML code with a DTD doctype declaration
  ============================================================================
  Don't remove attribute or change value of attributes in this node please.
  Exceptionally you can remove element 'code' from "cdata-section-elements" attibute
  to debug your XSLT transform, but when you call it from external tools, please replace it.-->
  <xsl:output method="xml" cdata-section-elements="code" encoding="ISO-8859-1" indent="yes"/>
  <!-- ======================================================================= -->
  <!-- These parameters are mandatorybut you can add your own parameters and declare them in External tools form. -->
  <xsl:param name="Version">1.0</xsl:param>
  <xsl:param name="ProjectFolder"/>
  <xsl:param name="ToolsFolder"/>
  <xsl:param name="LanguageFolder"/>
  <xsl:param name="InputClass"/>
  <xsl:param name="InputPackage"/>
  <!-- Add below your own parameters
  =======================================================================
  these variables are mandatory. Application will upgrade this for you if necessary. -->
  <xsl:variable name="FileLanguage">
    <xsl:value-of select="$LanguageFolder"/>\language.xml
  </xsl:variable>
  <xsl:variable name="PrefixList" select="translate(document($FileLanguage)//PrefixList/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixTypeList" select="translate(document($FileLanguage)//PrefixTypeList/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixStructProperty" select="translate(document($FileLanguage)//PrefixStructProperty/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixEnumProperty" select="translate(document($FileLanguage)//PrefixEnumProperty/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixMember" select="translate(document($FileLanguage)//PrefixMember/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixArray" select="translate(document($FileLanguage)//PrefixArray/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="SetParam" select="translate(document($FileLanguage)//SetParam/text(),'&#32;&#10;&#13;','')"/>
  <!-- Add below your own variables -->
  <!-- ======================================================================= -->
  <!--  This "xsl:template" block is mandatory, don't remove it please -->
  <xsl:template match="/root">
    <!--  you can add here a log trace of your own parameters if you need.
    This comment is optional, you can remove it. -->
    <xsl:comment>
      <xsl:text>
Version       :=</xsl:text>
      <xsl:value-of select="$Version"/>
      <xsl:text>
ProjectFolder :=</xsl:text>
      <xsl:value-of select="$ProjectFolder"/>
      <xsl:text>
ToolsFolder   :=</xsl:text>
      <xsl:value-of select="$ToolsFolder"/>
      <xsl:text>
LanguageFolder:=</xsl:text>
      <xsl:value-of select="$LanguageFolder"/>
      <xsl:text>
InputClass    :=</xsl:text>
      <xsl:value-of select="$InputClass"/>
      <xsl:text>
InputPackage  :=</xsl:text>
      <xsl:value-of select="$InputPackage"/>
    </xsl:comment>
    <!-- this element 'document' is mandatory-->
    <xsl:element name="document">
	  <!-- this attribute 'project' is not used by process, but only for your information.-->
      <xsl:attribute name="project">
        <xsl:value-of select="$ProjectFolder"/>
      </xsl:attribute>
      <!-- This 'xsl:choose' bloc is mandatory, please don't change it  -->
      <xsl:choose>
        <xsl:when test="$InputClass!=''">
          <xsl:apply-templates select="//root/package[descendant::class[@id=$InputClass]]" mode="Node"/>
          <xsl:apply-templates select="//root/class[@id=$InputClass]" mode="Code"/>
        </xsl:when>
        <xsl:when test="$InputPackage!=''">
          <xsl:apply-templates select="//root/package[@id=$InputPackage or descendant::package[@id=$InputPackage]]" mode="Node"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="." mode="Code"/>
          <xsl:apply-templates select="class" mode="Code"/>
          <xsl:apply-templates select="package" mode="Node"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <!--  This "xsl:template" block is mandatory, don't remove it please -->
  <xsl:template match="package" mode="Node">
    <!-- this "xsl:element" block is mandatory, please don't change it.-->
    <xsl:element name="package">
      <xsl:attribute name="name">
        <xsl:value-of select="@folder"/>
        <xsl:if test="not(@folder)">
          <xsl:value-of select="@name"/>
        </xsl:if>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="$InputClass!=''">
          <xsl:apply-templates select="package[descendant::class[@id=$InputClass]]" mode="Node"/>
          <xsl:apply-templates select="class[@id=$InputClass]" mode="Code"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="@id=$InputPackage">
              <xsl:apply-templates select="." mode="Code"/>
              <xsl:apply-templates select="class" mode="Code"/>
            </xsl:when>
            <xsl:when test="$InputPackage!=''">
              <xsl:apply-templates select="package[@id=$InputPackage or descendant::package[@id=$InputPackage]]" mode="Node"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="." mode="Code"/>
              <xsl:apply-templates select="class" mode="Code"/>
              <xsl:apply-templates select="package" mode="Node"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <!-- =======================================================================
  *
  *
  * Bellow this line, "xsl:template" blocks are not mandatory, except if you need
  * to generate some files
  *
  *
  ============================================================================ -->
  <xsl:template match="root" mode="Code">
    <!-- bloc "xsl:element" with atribute  'name': "code" is required if you need to generate a file here-->
    <xsl:element name="code">
      <!-- Attribute Merge (mandatory): no (generate .bak file),  yes (call Diff and merge tool of your choice) -->
      <xsl:attribute name="Merge">no</xsl:attribute>
      <!-- Attribute name (mandatory):  a filename of your choice, example: {@name}.mak
           File is generated in a folder beneath current package folder.
           Note: you can add a different folder path than current package folder
           but declare only relative path please! -->
      <xsl:attribute name="name">
        <xsl:value-of select="concat(@name,'.mak')"/>
      </xsl:attribute>
      <xsl:text>
      An example of code for 'root' node "</xsl:text>
      <xsl:value-of select="@name"/>".
    </xsl:element>
  </xsl:template>
  <!-- ============================================================================ -->
  <xsl:template match="package" mode="Code">
    <!-- bloc "xsl:element" with atribute  'name': "code" is required if you need to generate a file here-->
    <xsl:element name="code">
      <!-- Attribute Merge (mandatory): no (generate .bak file),  yes (call Diff and merge tool of your choice) -->
      <xsl:attribute name="Merge">no</xsl:attribute>
      <!-- Attribute name (mandatory):  a filename of your choice, example: {@name}.inc
           File is generated in a folder beneath current package folder.
           Note: you can add a different folder path than current package folder
           but declare only relative path please! -->
      <xsl:attribute name="name">
        <xsl:value-of select="concat(@name,'.inc')"/>
      </xsl:attribute>
      <xsl:text>An example of code for 'package' node "</xsl:text>
      <xsl:value-of select="@name"/>
      <xsl:text>" with UID '</xsl:text>
      <xsl:value-of select="@id"/>
      <xsl:text>'.</xsl:text>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="Code">
    <!-- bloc "xsl:element" with atribute  'name': "code" is required if you need to generate a file here-->
    <xsl:element name="code">
      <!-- Attribute Merge (mandatory): no (generate .bak file),  yes (call Diff and merge tool of your choice) -->
      <xsl:attribute name="Merge">no</xsl:attribute>
      <!-- Attribute name (mandatory):  a filename of your choice, example: {@name}.tmp
           File is generated in a folder beneath current package folder.
           Note: you can add a different folder path than current package folder
           but declare only relative path please! -->
      <xsl:attribute name="name">
        <xsl:value-of select="concat(@name,'.tmp')"/>
      </xsl:attribute>
      <xsl:text>
      An example of code for 'class' node "</xsl:text>
      <xsl:value-of select="@name"/>
      <xsl:text>" with UID '</xsl:text>
      <xsl:value-of select="@id"/>
      <xsl:text>'.</xsl:text>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  </xsl:stylesheet>







