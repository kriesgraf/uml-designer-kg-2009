<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- =============================================================================== -->
  <xsl:output indent="yes" method="xml" encoding="utf-8"/><!-- doctype-system="class-model.dtd"/-->
  <!-- =============================================================================== -->
  <xsl:param name="ParamName">ParamName</xsl:param>
  <!-- =============================================================================== -->
  <xsl:template match="/*">
    <xsl:if test="name()!='tagfile'">
      <xsl:message terminate="yes">This file is not a Doxygen TAG file !</xsl:message>
    </xsl:if>
    <xsl:element name="export">
      <xsl:attribute name="name">
        <xsl:value-of select="$ParamName"/>
      </xsl:attribute>
      <xsl:for-each select="//compound[@kind='file' and not(contains(name,'.cpp'))][class | member[@kind='typedef']]">
        <xsl:apply-templates select="class[@kind='class'] | member[@kind='typedef' and not(@protection)]"/>
      </xsl:for-each>
      <xsl:for-each select="//compound[@kind='class'][member/@kind='typedef'][not(@protection)]">
        <xsl:apply-templates select="member[@kind='typedef' and not(@protection)]"/>
        <xsl:apply-templates select="class" mode="Struct"/>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>
  <!-- =============================================================================== -->
  <xsl:template match="class" mode="Struct">
    <reference name="{substring-after(substring-after(text(),parent::compound/name),'::')}" type="typedef">
      <xsl:if test="parent::compound[@kind='class']">
        <xsl:apply-templates select="parent::compound/name"/>
      </xsl:if>
      <xsl:if test="parent::compound[@kind='file']">
        <xsl:attribute name="type">class</xsl:attribute>
        <xsl:attribute name="source"><xsl:value-of select="parent::compound/name"/></xsl:attribute>
      </xsl:if>
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
    </reference>
  </xsl:template>
  <!-- =============================================================================== -->
  <xsl:template match="member">
    <reference name="{name}" type="typedef">
      <xsl:if test="parent::compound[@kind='class']">
        <xsl:apply-templates select="parent::compound/name"/>
      </xsl:if>
      <xsl:if test="parent::compound[@kind='file']">
        <xsl:attribute name="type">class</xsl:attribute>
        <xsl:attribute name="source"><xsl:value-of select="parent::compound/name"/></xsl:attribute>
      </xsl:if>
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
    </reference>
  </xsl:template>
  <!-- =============================================================================== -->
  <xsl:template match="class">
    <xsl:if test="contains(.,'::')">
      <reference name="{substring-after(text(),'::')}" type="class">
        <xsl:attribute name="id">
          <xsl:value-of select="generate-id()"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="substring-before(text(),'::')"/>
        </xsl:attribute>
      </reference>
    </xsl:if>
    <xsl:if test="not(contains(.,'::'))">
      <reference name="{text()}" type="class">
        <xsl:attribute name="id">
          <xsl:value-of select="generate-id()"/>
        </xsl:attribute>
      </reference>
    </xsl:if>
  </xsl:template>
  <!-- =============================================================================== -->
  <xsl:template match="name">
    <xsl:if test="contains(.,'::')">
      <xsl:attribute name="class">
        <xsl:value-of select="substring-after(text(),'::')"/>
      </xsl:attribute>
      <xsl:attribute name="package">
        <xsl:value-of select="substring-before(text(),'::')"/>
      </xsl:attribute>
    </xsl:if>
    <xsl:if test="not(contains(.,'::'))">
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
    </xsl:if>
  </xsl:template>
  <!-- =============================================================================== -->
</xsl:stylesheet>