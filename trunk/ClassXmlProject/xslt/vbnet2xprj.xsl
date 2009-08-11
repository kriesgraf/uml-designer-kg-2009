<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ======================================================================= -->
  <xsl:output indent="yes" method="xml" encoding="utf-8" media-type="xprj"/>
  <!--xsl:output indent="yes" method="xml" encoding="utf-8" media-type="xprj" standalone="no" doctype-system="class-model.dtd"/-->
  <!-- ======================================================================= -->
  <xsl:param name="ProjectFolder"/>
  <xsl:param name="ToolsFolder"/>
  <xsl:param name="LanguageFolder"/>
  <!-- ============================================================================== -->
  <xsl:template match="/project">
    <root name="VbCodeRevEng">
      <generation destination="{@folder}" language="1"/>
      <comment brief="Brief description">Detailed comment</comment>
      <xsl:apply-templates select="*"/>
    </root>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="imports"/>
  <!-- ============================================================================== -->
  <xsl:template match="root">
    <xsl:apply-templates select="*"/>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="class">
    <xsl:variable name="Visibility">
      <xsl:choose>
        <xsl:when test="@visibility='Public'">package</xsl:when>
        <xsl:otherwise>common</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Implementation">
      <xsl:choose>
        <xsl:when test="@kind='Interface'">abstract</xsl:when>
        <xsl:when test="contains(@other,'NotInheritable')">final</xsl:when>
        <xsl:when test="contains(@other,'MustInherit')">root</xsl:when>
        <xsl:otherwise>simple</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <class name="{@name}" implementation="{$Implementation}" visibility="{$Visibility}" constructor="no" destructor="no" inline="none" id="{generate-id()}">
      <xsl:apply-templates select="inherited"/>
      <comment brief="Brief description">Detailed comment</comment>
      <xsl:apply-templates select="*[not(self::inherited)]">
        <xsl:with-param name="Implementation" select="$Implementation"/>
      </xsl:apply-templates>
    </class>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="region">
    <xsl:param name="Implementation"/>
    <xsl:apply-templates select="*">
      <xsl:with-param name="Implementation" select="$Implementation"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="inherited">
    <xsl:copy>
      <xsl:variable name="Name" select="@name"/>
      <xsl:attribute name="idref">
        <xsl:apply-templates select="//class[@name=$Name]" mode="IDREF"/>
      </xsl:attribute>
    </xsl:copy>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="*" mode="IDREF">
    <xsl:value-of select="generate-id()"/>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="typedef">
    <xsl:copy-of select="."/>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="property | attribute">
    <xsl:param name="Implementation"/>
    <xsl:variable name="Overridable">
      <xsl:choose>
        <xsl:when test="$Implementation='final'">no</xsl:when>
        <xsl:when test="$Implementation='abstract'">yes</xsl:when>
        <xsl:when test="contains(@other,'MustOverride')">yes</xsl:when>
        <xsl:when test="contains(@other,'Overridable')">yes</xsl:when>
        <xsl:when test="contains(@other,'Overrides')">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Member">
      <xsl:choose>
        <xsl:when test="contains(@other,'Shared')">class</xsl:when>
        <xsl:otherwise>object</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Attribute">
      <xsl:choose>
        <xsl:when test="self::attribute">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <property attribute="{$Attribute}" overridable="{$Overridable}" name="{@name}" member="{$Member}" num-id="{position()}">
      <type desc="{@type}" level="0" by="val" modifier="var"/>
      <variable range="private">
        <xsl:if test="@size!=''">
          <xsl:attribute name="size">10</xsl:attribute>
        </xsl:if>
      </variable>
      <comment>Insert here a comment</comment>
      <xsl:variable name="Range">
        <xsl:choose>
          <xsl:when test="@visibility='Private'">private</xsl:when>
          <xsl:when test="@visibility='Protected'">protected</xsl:when>
          <xsl:when test="@visibility='Protected Friend'">protected</xsl:when>
          <xsl:otherwise>public</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:choose>
        <xsl:when test="get">
          <get inline="no" by="val" modifier="var" range="{$Range}"/>
        </xsl:when>
        <xsl:otherwise>
          <get inline="no" by="val" modifier="var" range="no"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:choose>
        <xsl:when test="set">
          <set inline="no" by="val" range="{$Range}"/>
        </xsl:when>
        <xsl:otherwise>
          <set inline="no" by="val" range="no"/>
        </xsl:otherwise>
      </xsl:choose>
    </property>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="method">
    <xsl:param name="Implementation"/>
    <xsl:variable name="Member">
      <xsl:choose>
        <xsl:when test="contains(@other,'Shared')">class</xsl:when>
        <xsl:otherwise>object</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Range">
      <xsl:choose>
        <xsl:when test="@visibility='Private'">private</xsl:when>
        <xsl:when test="@visibility='Protected'">protected</xsl:when>
        <xsl:when test="@visibility='Protected Friend'">protected</xsl:when>
        <xsl:otherwise>public</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="MethodImpl">
      <xsl:choose>
        <xsl:when test="$Implementation='abstract'">abstract</xsl:when>
        <xsl:when test="contains(@other,'NotOverridable Overrides')">final</xsl:when>
        <xsl:when test="contains(@other,'Overrides')">virtual</xsl:when>
        <xsl:when test="contains(@other,'Overridable')">root</xsl:when>
        <xsl:when test="contains(@other,'MustOverride')">root</xsl:when>
        <xsl:otherwise>simple</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Return">
      <xsl:choose>
        <xsl:when test="@type='Sub'">void</xsl:when>
        <xsl:when test="@return">
          <xsl:value-of select="@return"/>
        </xsl:when>
        <xsl:otherwise>void</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <method modifier="var" inline="no" name="{@name}" constructor="no"
            member="{$Member}" implementation="{$MethodImpl}" num-id="{position()}">
      <return>
        <type desc="{$Return}" level="0" by="val" modifier="var"/>
        <variable range="{$Range}"/>
        <comment>Return comment</comment>
      </return>
      <comment brief="Brief description">Insert here a comment</comment>
      <xsl:call-template name="PARAMS">
        <xsl:with-param name="Params" select="@params"/>
        <xsl:with-param name="Types" select="@types"/>
        <xsl:with-param name="Values" select="@values"/>
      </xsl:call-template>
    </method>
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
  <!-- ============================================================================== -->
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
  <!-- ============================================================================== -->
  <xsl:template match="text()"/>
  <!-- ============================================================================== -->
</xsl:stylesheet>





























