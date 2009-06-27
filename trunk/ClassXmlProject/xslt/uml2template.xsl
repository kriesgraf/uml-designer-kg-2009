﻿<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
  <!-- ======================================================================= -->
  <xsl:output method="xml" cdata-section-elements="code" encoding="ISO-8859-1"/>
  <!-- ======================================================================= -->
  <xsl:param name="ProjectFolder"/>
  <xsl:param name="ToolsFolder"/>
  <xsl:param name="LanguageFolder"/>
  <xsl:param name="InputClass"/>
  <xsl:param name="InputPackage"/>
  <!-- ======================================================================= -->
  <xsl:variable name="FileLanguage"><xsl:value-of select="$LanguageFolder"/>\language.xml</xsl:variable>
  <xsl:variable name="PrefixList" select="translate(document($FileLanguage)//PrefixList/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixTypeList" select="translate(document($FileLanguage)//PrefixTypeList/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixStructProperty" select="translate(document($FileLanguage)//PrefixStructProperty/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixEnumProperty" select="translate(document($FileLanguage)//PrefixEnumProperty/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixMember" select="translate(document($FileLanguage)//PrefixMember/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixArray" select="translate(document($FileLanguage)//PrefixArray/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="SetParam" select="translate(document($FileLanguage)//SetParam/text(),'&#32;&#10;&#13;','')"/>
  <!-- ======================================================================= -->
  <xsl:template match="/root">
    <xsl:comment>
ToolsFolder   :=<xsl:value-of select="$ToolsFolder"/>
LanguageFolder:=<xsl:value-of select="$LanguageFolder"/>
InputClass    :=<xsl:value-of select="$InputClass"/>
InputPackage  :=<xsl:value-of select="$InputPackage"/>
    </xsl:comment>
    <xsl:element name="document">
      <!-- CAUTION: do not change this version, application will upgrade this for you if necessary -->
      <xsl:attribute name="version">1.0</xsl:attribute>
      <!-- Possible "method" value: code, batch -->
      <xsl:attribute name="method">code</xsl:attribute>
      <xsl:attribute name="project">
        <xsl:value-of select="$ProjectFolder"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="$InputClass!=''">
          <xsl:apply-templates select="//root/package[descendant::class[@id=$InputClass]]" mode="Code"/>
          <xsl:apply-templates select="//root/class[@id=$InputClass]" mode="Code"/>
        </xsl:when>
        <xsl:when test="$InputPackage!=''">
          <xsl:apply-templates select="//root/package[@id=$InputPackage or descendant::package[@id=$InputPackage]]" mode="Code"/>
        </xsl:when>
        <xsl:otherwise>
          <!-- The element "code" declare a code source file to generate. 
               This file could contains programming language to compile, or a DOS script to execute  -->
          <xsl:element name="code">
            <!-- Attribute Merge (mandatory): no (generate .bak file),  yes (call Diff and merge tool of your choice) -->
            <xsl:attribute name="Merge">no</xsl:attribute>
            <!-- Attribute name (mandatory):  a filename of your choice, a "Makefile" for example: {@name}.mak 
                 File is generated in the folder you have declared in your project properties. -->
            <xsl:attribute name="name"><xsl:value-of select="@name"/>.mak</xsl:attribute>
            This is a template. Please implement your own code here for your "root" node if necessary.
          </xsl:element>
          <xsl:apply-templates select="class" mode="Code"/>
          <xsl:apply-templates select="package" mode="Code"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="package" mode="Code">
    <xsl:variable name="PackageName">
      <xsl:value-of select="@folder"/>
      <xsl:if test="not(@folder)">
        <xsl:value-of select="@name"/>
      </xsl:if>
    </xsl:variable>
    <xsl:element name="package">
      <!-- Attribute name (mandatory):  a package folder of your choice, example: {$PackageName}
           Each class object declared inside this one is generated in this folder
      -->
      <xsl:attribute name="name"><xsl:value-of select="$PackageName"/></xsl:attribute>
      <xsl:choose>
        <xsl:when test="$InputClass!=''">
          <xsl:apply-templates select="package[descendant::class[@id=$InputClass]]" mode="Code"/>
          <xsl:apply-templates select="class[@id=$InputClass]" mode="Code"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:element name="code">
            <!-- Attribute Merge (mandatory): no (generate .bak file),  yes (call Diff and merge tool of your choice) -->
            <xsl:attribute name="Merge">no</xsl:attribute>
            <!-- Attribute name (mandatory):  a file that contains main declaration for while classes inside package.
                 Example: {$PackageName}.h -->
            <xsl:attribute name="name"><xsl:value-of select="$PackageName"/>.h</xsl:attribute>
            This is a template. Please implement your own code here for your "package" nodes if necessary.
          </xsl:element>
          <xsl:choose>
            <xsl:when test="@id=$InputPackage">
              <xsl:apply-templates select="class" mode="Code"/>
            </xsl:when>
            <xsl:when test="$InputPackage!=''">
              <xsl:apply-templates select="package[@id=$InputPackage or descendant::package[@id=$InputPackage]]" mode="Code"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="class" mode="Code"/>
              <xsl:apply-templates select="package" mode="Code"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="class" mode="Code">
    <xsl:element name="code">
      <!-- Attribute Merge (mandatory): no (generate .bak file),  yes (call Diff and merge tool of your choice) -->
      <xsl:attribute name="Merge">no</xsl:attribute>
      <!-- Attribute name (mandatory):  a filename of your choice, example: {@name}.tmp 
           File is generated in the folder you have declared in your project properties + the package folder
           when class is declared inside this one.
      -->
      <xsl:attribute name="name"><xsl:value-of select="@name"/>.tmp</xsl:attribute>
      This is a template. Please implement your own code here for your "class" nodes if necessary.
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
</xsl:stylesheet>