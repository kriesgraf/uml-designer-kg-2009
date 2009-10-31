<?xml version="1.0" encoding="UTF-8"?>
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
  <!-- ======================================================================= -->
  <xsl:template match="class">
    <xsl:element name="document">
      <xsl:apply-templates select="." mode="Code"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="package">
    <xsl:element name="document">
      <xsl:apply-templates select="." mode="Code"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef | property | method">
    <xsl:element name="document">
      No code generation at this level
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="import|relationship">
    <xsl:element name="document">
      No code generation for this node
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="/root">
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
          <!--xsl:comment>InputClass=<xsl:value-of select="$InputClass"/></xsl:comment-->
          <xsl:apply-templates select="//root/package[descendant::class[@id=$InputClass]]" mode="Code"/>
          <xsl:apply-templates select="//root/class[@id=$InputClass]" mode="Code"/>
        </xsl:when>
        <xsl:when test="$InputPackage!=''">
          <!--xsl:comment>InputPackage=<xsl:value-of select="$InputPackage"/></xsl:comment-->
          <xsl:apply-templates select="//root/package[@id=$InputPackage or descendant::package[@id=$InputPackage]]" mode="Code"/>
        </xsl:when>
        <xsl:otherwise>
          <!--xsl:comment>Otherwise</xsl:comment-->
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
      <xsl:copy-of select="@name | @id"/>
      <xsl:choose>
        <xsl:when test="$InputClass!=''">
          <xsl:apply-templates select="package[descendant::class[@id=$InputClass]]" mode="Code"/>
          <xsl:apply-templates select="class[@id=$InputClass]" mode="Code"/>
        </xsl:when>
        <xsl:when test="$InputPackage!=''">
          <xsl:if test="@id=$InputPackage">
            <xsl:apply-templates select="class" mode="Code"/>
          </xsl:if>
          <xsl:apply-templates select="package[@id=$InputPackage or descendant::package[@id=$InputPackage]]" mode="Code"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="class" mode="Code"/>
          <xsl:apply-templates select="package" mode="Code"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="Code">
    <xsl:element name="code">
      <!-- Possible "Merge" value: no, yes (To preserve your previous generated code) -->
      <xsl:attribute name="Merge">yes</xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>.java
      </xsl:attribute>
      Not yet implemented
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
</xsl:stylesheet>