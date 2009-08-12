<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
<!-- ======================================================================= -->
  <xsl:key match="element-type" name="include" use="@name"/>
  <!-- ======================================================================= -->
  <xsl:param name="Language">
    <xsl:value-of select="$LanguageFolder"/>
    <xsl:text>\LanguageVbasic.xml</xsl:text>
  </xsl:param>
  <!-- ======================================================================= -->
  <xsl:variable name="Classes">
    <xsl:apply-templates select="//class | //typedef" mode="UnknownTypes"/>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="UnknownTypes1">
    <xsl:apply-templates select="//method[@return!=''] | //property | //attribute | //typedef" mode="UnknownTypes"/>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="UnknownTypes">
    <xsl:for-each select="msxsl:node-set($UnknownTypes1)//*[generate-id()=generate-id(key('include',@name)[1])]">
      <xsl:sort select="@name"/>
      <xsl:variable name="Description">
        <xsl:call-template name="SimpleTypes">
          <xsl:with-param name="Label" select="@name"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Idref">
        <xsl:call-template name="ClassMember">
          <xsl:with-param name="Label" select="@name"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:copy>
        <xsl:copy-of select="@*"/>
        <xsl:choose>
          <xsl:when test="$Description!=''">
            <xsl:attribute name="desc">
              <xsl:value-of select="$Description"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="$Idref!=''">
            <xsl:attribute name="idref">
              <xsl:value-of select="$Idref"/>
            </xsl:attribute>
          </xsl:when>
        </xsl:choose>
      </xsl:copy>
    </xsl:for-each>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:template name="SimpleTypes">
    <xsl:param name="Label"/>
    <xsl:value-of select="document($Language)//*[@implementation=$Label]/@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="ClassMember">
    <xsl:param name="Label"/>
    <xsl:value-of select="msxsl:node-set($Classes)//*[@name=$Label]/@id"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class | typedef" mode="UnknownTypes">
    <xsl:copy>
      <xsl:copy-of select="@*"/>
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef" mode="UnknownTypes">
    <xsl:apply-templates select="element" mode="UnknownTypes"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="property | attribute | element" mode="UnknownTypes">
    <element-type node="{name()}" name="{@type}"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="method" mode="UnknownTypes">
    <element-type node="{name()}" name="{@return}"/>
    <xsl:variable name="ListParams">
      <xsl:call-template name="PARAMS">
        <xsl:with-param name="Params" select="@params"/>
        <xsl:with-param name="Types" select="@types"/>
        <xsl:with-param name="Values" select="@values"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:for-each select="msxsl:node-set($ListParams)//param">
      <element-type node="{name()}" name="{type/@desc}"/>
    </xsl:for-each>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template name="PARAMS">
    <xsl:param name="Params"/>
    <xsl:param name="Types"/>
    <xsl:param name="Values"/>
    <xsl:choose>
      <xsl:when test="contains($Params,',')">
        <xsl:call-template name="ImplementsPARAMS">
          <xsl:with-param name="Param" select="substring-before($Params,',')"/>
          <xsl:with-param name="Type" select="substring-before($Types,',')"/>
          <xsl:with-param name="Value" select="substring-before($Values,',')"/>
        </xsl:call-template>
        <xsl:call-template name="PARAMS">
          <xsl:with-param name="Params" select="substring-after($Params,',')"/>
          <xsl:with-param name="Types" select="substring-after($Types,',')"/>
          <xsl:with-param name="Values" select="substring-after($Values,',')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$Params=''"/>
      <xsl:otherwise>
        <xsl:call-template name="ImplementsPARAMS">
          <xsl:with-param name="Param" select="$Params"/>
          <xsl:with-param name="Type" select="$Types"/>
          <xsl:with-param name="Value" select="$Values"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="ImplementsPARAMS">
    <xsl:param name="Param"/>
    <xsl:param name="Type"/>
    <xsl:param name="Value"/>
    <xsl:variable name="Name">
      <xsl:choose>
        <xsl:when test="starts-with($Param,'Optional')">
          <xsl:value-of select="substring-after($Param,'Optional ')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$Param"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <param num-id="0" name="{substring-after($Name,' ')}">
      <type desc="{$Type}" modifier="var" level="0">
        <xsl:attribute name="by">
          <xsl:choose>
            <xsl:when test="starts-with($Name,'ByRef')">
              <xsl:text>ref</xsl:text>
            </xsl:when>
            <xsl:otherwise>val</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </type>
      <variable range="private">
        <xsl:if test="$Value!=''">
          <xsl:attribute name="value">
            <xsl:value-of select="$Value"/>
          </xsl:attribute>
        </xsl:if>
      </variable>
      <comment>Comment</comment>
    </param>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="text()"/>
  <!-- ======================================================================= -->
</xsl:stylesheet>












