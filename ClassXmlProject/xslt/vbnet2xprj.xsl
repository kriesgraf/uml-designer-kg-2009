<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ======================================================================= -->
  <xsl:output indent="yes" method="xml" encoding="utf-8" media-type="xprj" standalone="no" doctype-system="class-model.dtd"/>
  <!-- ======================================================================= -->
  <xsl:param name="ProjectFolder"/>
  <xsl:param name="ToolsFolder"/>
  <xsl:param name="LanguageFolder">E:\Documents\Mes projets\uml-designer-kg-2009\ClassXmlProject\xslt</xsl:param>
  <xsl:param name="LanguageID">2</xsl:param>
  <!-- ======================================================================= -->
  <xsl:include href="vbnet-syntax.xsl"/>
  <!-- ============================================================================== -->
  <xsl:template match="/project">
    <root>
      <xsl:attribute name="name">
        <xsl:choose>
          <xsl:when test="@name!=''">
            <xsl:value-of select="@name"/>
          </xsl:when>
          <xsl:otherwise>VbCodeRevEng</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <generation destination="{$ProjectFolder}" language="{$LanguageID}"/>
      <comment brief="Brief description">Detailed comment</comment>
      <!--
      <ICI>
        <xsl:copy-of select="$UnknownTypes"/>
      </ICI>
       -->
      <xsl:call-template name="Imports"/>
      <import name="Imports_to_sort" visibility="package">
        <export name="Imports_to_sort">
          <xsl:call-template name="UnknownImports"/>
        </export>
      </import>
      <xsl:apply-templates select="*[not(self::imports)]"/>
    </root>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="package">
    <xsl:copy>
      <xsl:copy-of select="@name"/>
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
      <comment brief="Brief comment">Detailed comment</comment>
      <xsl:apply-templates select="*"/>
    </xsl:copy>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="imports">
    <import name="{@name}" visibility="package">
      <export name="{@name}"/>
    </import>
    <xsl:for-each select="node-import">
      <import name="{.}" visibility="package">
        <export name="{.}"/>
      </import>
    </xsl:for-each>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="element-type" mode="Imports">
    <reference name="{@name}" type="class" id="{@idref}" container="{@container}"/>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="class">
    <xsl:variable name="ShouldInherit">
      <xsl:apply-templates select="." mode="ShouldInherit"/>
    </xsl:variable>
    <!--ICI>
      <xsl:copy-of select="$ShouldInherit"/>
    </ICI-->
    <xsl:choose>
      <xsl:when test="ancestor::class">
        <xsl:variable name="Comment">
          <xsl:choose>
            <xsl:when test="preceding-sibling::*[position()=1 and name()='vb-doc']">
              <xsl:apply-templates select="preceding-sibling::*[position()=1 and name()='vb-doc']" mode="Short"/>
            </xsl:when>
            <xsl:otherwise>Insert here a comment</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="Visibility">
          <xsl:choose>
            <xsl:when test="@visibility='Public'">public</xsl:when>
            <xsl:when test="starts-with(@visibility,'Protected')">protected</xsl:when>
            <xsl:otherwise>private</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="inherited[@template]">
            <typedef name="{@name}" id="{generate-id()}">
              <type by="val" modifier="var" level="0">
                <xsl:apply-templates select="inherited" mode="Type"/>
                <list>
                  <xsl:apply-templates select="inherited" mode="List"/>
                </list>
              </type>
              <variable range="{$Visibility}"/>
              <comment>
                <xsl:value-of select="$Comment"/>
              </comment>
            </typedef>
          </xsl:when>
          <xsl:otherwise>
            <typedef name="{@name}" id="{generate-id()}">
              <type by="val" modifier="var" level="0">
                <xsl:apply-templates select="inherited" mode="Typedef"/>
              </type>
              <variable range="{$Visibility}"/>
              <comment>
                <xsl:value-of select="$Comment"/>
              </comment>
            </typedef>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
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
          <xsl:choose>
            <xsl:when test="preceding-sibling::*[position()=1 and name()='vb-doc']">
              <xsl:apply-templates select="preceding-sibling::*[position()=1 and name()='vb-doc']" mode="Full"/>
            </xsl:when>
            <xsl:otherwise>
              <comment brief="Brief description">Detailed comment</comment>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:apply-templates select="descendant::typedef | descendant::class">
            <xsl:with-param name="Implementation" select="$Implementation"/>
          </xsl:apply-templates>
          <xsl:apply-templates select="descendant::attribute | descendant::property">
            <xsl:with-param name="Implementation" select="$Implementation"/>
          </xsl:apply-templates>
          <xsl:apply-templates select="descendant::method">
            <xsl:with-param name="Implementation" select="$Implementation"/>
            <xsl:with-param name="ShouldInherit" select="$ShouldInherit"/>
          </xsl:apply-templates>
        </class>
      </xsl:otherwise>
    </xsl:choose>
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
      <xsl:attribute name="range">public</xsl:attribute>
      <xsl:attribute name="idref">
        <xsl:call-template name="SearchMember">
          <xsl:with-param name="Label" select="@name"/>
        </xsl:call-template>
      </xsl:attribute>
    </xsl:copy>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="typedef">
    <xsl:variable name="Visibility">
      <xsl:choose>
        <xsl:when test="@visibility='Public'">public</xsl:when>
        <xsl:when test="starts-with(@visibility,'Protected')">protected</xsl:when>
        <xsl:otherwise>private</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:copy>
      <xsl:copy-of select="@name"/>
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
      <type by="val" modifier="var" level="0">
        <xsl:if test="element">
          <xsl:attribute name="struct">struct</xsl:attribute>
        </xsl:if>
        <xsl:apply-templates select="*"/>
      </type>
      <variable range="{$Visibility}"/>
      <comment>Insert here a comment</comment>
    </xsl:copy>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="element">
    <element num-id="0" modifier="var" level="0">
      <xsl:copy-of select="@name"/>
      <xsl:apply-templates select="@type" mode="Type"/>
    </element>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="enumvalue">
    <enumvalue name="{@name}" id="{generate-id()}">
      <xsl:text>insert here a comment</xsl:text>
    </enumvalue>
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
      <type level="0" by="val" modifier="var">
        <xsl:apply-templates select="@type" mode="Type"/>
      </type>
      <variable range="private">
        <xsl:if test="@size!=''">
          <xsl:attribute name="size">10</xsl:attribute>
        </xsl:if>
      </variable>
      <comment>
        <xsl:choose>
          <xsl:when test="preceding-sibling::*[position()=1 and name()='vb-doc']">
            <xsl:apply-templates select="preceding-sibling::*[position()=1 and name()='vb-doc']" mode="Short"/>
          </xsl:when>
          <xsl:otherwise>Insert here a comment</xsl:otherwise>
        </xsl:choose>
      </comment>
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
    <xsl:param name="ShouldInherit"/>
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
    <xsl:variable name="Overrides">
      <xsl:apply-templates select="." mode="Overrides">
        <xsl:with-param name="ShouldInherit" select="$ShouldInherit"/>
      </xsl:apply-templates>
    </xsl:variable>
    <method modifier="var" inline="no" member="{$Member}" num-id="{position()}">
      <xsl:choose>
        <xsl:when test="@name!='New'">
          <xsl:attribute name="constructor">no</xsl:attribute>
          <xsl:attribute name="name">
            <xsl:value-of select="@name"/>
          </xsl:attribute>
          <xsl:attribute name="implementation">
            <xsl:value-of select="$MethodImpl"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="constructor">
            <xsl:value-of select="$Range"/>
          </xsl:attribute>
          <xsl:attribute name="implementation">simple</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:if test="$Overrides!=''">
        <xsl:attribute name="overrides">
          <xsl:value-of select="$Overrides"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="@name!='New'">
        <return>
          <type level="0" by="val" modifier="var">
            <xsl:choose>
              <xsl:when test="@type='Sub'">
                <xsl:attribute name="desc">void</xsl:attribute>
              </xsl:when>
              <xsl:when test="@return">
                <xsl:apply-templates select="@return" mode="Type"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="desc">void</xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </type>
          <variable range="{$Range}"/>
          <comment>
            <xsl:choose>
              <xsl:when test="preceding-sibling::*[position()=1 and name()='vb-doc']">
                <xsl:apply-templates select="preceding-sibling::*[position()=1 and name()='vb-doc']" mode="Return"/>
              </xsl:when>
              <xsl:otherwise>Return comment</xsl:otherwise>
            </xsl:choose>
          </comment>
        </return>
      </xsl:if>
      <xsl:choose>
        <xsl:when test="preceding-sibling::*[position()=1 and name()='vb-doc']">
          <xsl:apply-templates select="preceding-sibling::*[position()=1 and name()='vb-doc']" mode="Full"/>
        </xsl:when>
        <xsl:otherwise>
          <comment brief="Brief description">Detailed comment</comment>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:call-template name="AllParams">
        <xsl:with-param name="Comments">
          <xsl:copy-of select="preceding-sibling::*[position()=1 and name()='vb-doc']"/>
        </xsl:with-param>
      </xsl:call-template>
    </method>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="ImplementsPARAMS">
    <xsl:param name="Param"/>
    <xsl:param name="Type"/>
    <xsl:param name="Value"/>
    <xsl:param name="Comments"/>
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
      <type modifier="var" level="0" desc="{$Type}">
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
      <xsl:call-template name="ParamComment">
        <xsl:with-param name="Comments" select="$Comments"/>
        <xsl:with-param name="ParamName" select="substring-after($Name,' ')"/>
      </xsl:call-template>
    </param>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="vb-doc" mode="Full">
    <comment brief="{summary}">
      <xsl:value-of select="remarks"/>
    </comment>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="vb-doc" mode="Short">
    <xsl:value-of select="summary"/>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="vb-doc" mode="Return">
    <xsl:value-of select="returns"/>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template match="vb-doc" mode="Param">
    <xsl:param name="ParamName"/>
    <comment>
      <xsl:value-of select="param[@name=$ParamName]"/>
    </comment>
  </xsl:template>
  <!-- ============================================================================== -->
</xsl:stylesheet>












































































