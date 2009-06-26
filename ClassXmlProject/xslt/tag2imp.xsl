<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- =============================================================================== -->
  <xsl:output indent="yes" method="xml" encoding="utf-8"/><!-- doctype-system="class-model.dtd"/-->
  <!-- =============================================================================== -->
  <xsl:param name="ParamName">ParamName</xsl:param>
  <xsl:param name="Language">0</xsl:param>
  <!-- =============================================================================== -->
  <xsl:variable name="Separator">
  <xsl:choose>
    <xsl:when test="$Language='0'">::</xsl:when>
    <xsl:otherwise>.</xsl:otherwise>
  </xsl:choose>
  </xsl:variable>
  <!-- =============================================================================== -->
  <xsl:template match="/*">
    <xsl:if test="name()!='tagfile'">
      <xsl:message terminate="yes">This file is not a Doxygen TAG file !</xsl:message>
    </xsl:if>
    <xsl:element name="export">
      <xsl:attribute name="name">
        <xsl:value-of select="$ParamName"/>
      </xsl:attribute>
      <xsl:for-each select="//compound[@kind='file' and not(contains(name,'.cpp'))]">
        <xsl:apply-templates select="class[@kind='class']"/>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>
  <!-- =============================================================================== -->
  <xsl:template match="class">
    <xsl:variable name="ClassName" select="text()"/>
    <xsl:variable name="ClassNode">
    <xsl:choose>
         <xsl:when test="//compound[@kind='class' and name=$ClassName and member[@kind='function' and @virtualness='pure']]">interface</xsl:when>
         <xsl:otherwise>reference</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:element name="{$ClassNode}">
      <xsl:attribute name="name">
        <xsl:choose>
          <xsl:when test="contains($ClassName,$Separator)">
            <xsl:value-of select="substring-after($ClassName,$Separator)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ClassName"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:if test="$ClassNode='reference'">
        <xsl:attribute name="type">class</xsl:attribute>
      </xsl:if>
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
      <xsl:variable name="Package">
        <xsl:value-of select="substring-before($ClassName,$Separator)"/>
      </xsl:variable>
      <xsl:if test="$Package!=''">
        <xsl:attribute name="package">
          <xsl:value-of select="$Package"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="//compound[@kind='class' and name=$ClassName]/member[@kind='function' and @virtualness='pure']" mode="Method"/>
    </xsl:element>
    <xsl:apply-templates select="//compound[@kind='class' and name=$ClassName]/member[@kind='typedef' and not(@protection)]" mode="Typedef"/>
  </xsl:template>
  <!-- =============================================================================== -->
  <xsl:template match="member" mode="Typedef">
    <xsl:variable name="TypeName">
      <xsl:choose>
        <xsl:when test="starts-with(name,parent::compound/name)">
          <xsl:value-of select="substring-after(substring-after(text(),parent::compound/name),$Separator)"/>
        </xsl:when>
        <xsl:otherwise><xsl:value-of select="name"/></xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <reference name="{$TypeName}" type="typedef">
      <xsl:if test="parent::compound[@kind='class']">
        <xsl:apply-templates select="parent::compound/name"/>
      </xsl:if>
      <xsl:if test="parent::compound[@kind='file']">
        <xsl:attribute name="type">class</xsl:attribute>
      </xsl:if>
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
    </reference>
  </xsl:template>
  <!-- =============================================================================== -->
  <xsl:template match="member" mode="Method">
    <method modifier="var" inline="no" name="{name}" constructor="no"
            member="object" implementation="abstract" num-id="{position()}">
      <return><xsl:value-of select="type"/></return>
      <comment/>
      <xsl:copy-of select="arglist"/>
    </method>
  </xsl:template>
  <!-- =============================================================================== -->
  <xsl:template match="name">
    <xsl:if test="contains(.,$Separator)">
      <xsl:attribute name="class">
        <xsl:value-of select="substring-after(text(),$Separator)"/>
      </xsl:attribute>
      <xsl:variable name="Package">
        <xsl:value-of select="substring-before(text(),$Separator)"/>
      </xsl:variable>
      <xsl:if test="$Package!=''">
        <xsl:attribute name="package">
          <xsl:value-of select="$Package"/>
        </xsl:attribute>
      </xsl:if>
    </xsl:if>
    <xsl:if test="not(contains(.,$Separator))">
      <xsl:attribute name="class">
        <xsl:value-of select="text()"/>
      </xsl:attribute>
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
    </xsl:if>
  </xsl:template>
  <!-- =============================================================================== -->
</xsl:stylesheet>

