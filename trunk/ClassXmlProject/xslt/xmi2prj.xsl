<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0"
     xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
     xmlns:msxsl="urn:schemas-microsoft-com:xslt"
     xmlns:uml="http://schema.omg.org/spec/UML/2.1"
     xmlns:xmi="http://schema.omg.org/spec/XMI/2.1">
<!-- ======================================================================= -->
  <xsl:output indent="yes" method="xml" encoding="utf-8" media-type="xprj" doctype-system="class-model.dtd"/>
  <!--============================================================================== -->
  <xsl:attribute-set name="Type">
    <xsl:attribute name="level">0</xsl:attribute>
    <xsl:attribute name="by">val</xsl:attribute>
    <xsl:attribute name="modifier">var</xsl:attribute>
  </xsl:attribute-set>
  <!--============================================================================== -->
  <xsl:attribute-set name="RelationParent">
    <xsl:attribute name="level">0</xsl:attribute>
    <xsl:attribute name="range">no</xsl:attribute>
    <xsl:attribute name="cardinal">01</xsl:attribute>
  </xsl:attribute-set>
  <!--============================================================================== -->
  <xsl:attribute-set name="Element">
    <xsl:attribute name="name">
      <xsl:value-of select="@name"/>
    </xsl:attribute>
    <xsl:attribute name="num-id">
      <xsl:value-of select="position()"/>
    </xsl:attribute>
    <xsl:attribute name="level">0</xsl:attribute>
    <xsl:attribute name="modifier">var</xsl:attribute>
  </xsl:attribute-set>
  <!--============================================================================== -->
  <xsl:attribute-set name="Node">
    <xsl:attribute name="name">
      <xsl:value-of select="@name"/>
    </xsl:attribute>
    <xsl:attribute name="id">
      <xsl:value-of select="@xmi:id"/>
    </xsl:attribute>
  </xsl:attribute-set>
  <!--============================================================================== -->
  <xsl:variable name="DataTypes">
    <xsl:for-each select="//packagedElement[@xmi:type='uml:DataType']">
      <xsl:element name="descriptor">
        <xsl:attribute name="id">
          <xsl:value-of select="@xmi:id"/>
        </xsl:attribute>
        <xsl:copy-of select="@name"/>
      </xsl:element>
    </xsl:for-each>
  </xsl:variable>
  <!--============================================================================== -->
  <xsl:template match="/*">
    <xsl:if test="@xmi:version!='2.1'">
      <xsl:message terminate="yes">Expected XMI/UML 2.1 !</xsl:message>
    </xsl:if>
    <xsl:element name="root">
      <xsl:attribute name="version">1.2</xsl:attribute>
      <xsl:attribute name="name">XMI UML 2.1</xsl:attribute>
      <xsl:element name="generation">
        <xsl:attribute name="destination">C:\</xsl:attribute>
        <xsl:attribute name="language">0</xsl:attribute>
      </xsl:element>
      <xsl:element name="comment">
        <xsl:attribute name="brief">A brief comment</xsl:attribute>A detailed comment
      </xsl:element>
      <xsl:copy-of select="$DataTypes"/>
      <!--xsl:apply-templates select="uml:Model/*"/-->
      <xsl:apply-templates select="//packagedElement[@xmi:type='uml:Association']"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="packagedElement">
    <xsl:choose>
      <xsl:when test="@xmi:type='uml:Package'">
        <xsl:call-template name="Package"/>
      </xsl:when>
      <xsl:when test="@xmi:type='uml:Class'">
        <xsl:call-template name="Class"/>
      </xsl:when>
      <xsl:when test="@xmi:type='uml:Association'">
        <xsl:call-template name="Association"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedComment">
    <xsl:call-template name="Comment"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Comment">
    <xsl:element name="comment">
      <xsl:attribute name="brief">
        <xsl:value-of select="@body"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="nestedClassifier">
    <xsl:element name="typedef" use-attribute-sets="Node">
      <xsl:element name="type">
        <xsl:attribute name="level">0</xsl:attribute>
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="modifier">var</xsl:attribute>
        <xsl:for-each select="ownedAttribute">
          <xsl:element name="element" use-attribute-sets="Element">
            <xsl:value-of select="ownedComment/@body"/>
          </xsl:element>
        </xsl:for-each>
      </xsl:element>
      <xsl:element name="variable">
        <xsl:attribute name="range">
          <xsl:choose>
            <xsl:when test="@visibility='package'">public</xsl:when>
            <xsl:otherwise>private</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </xsl:element>
      <xsl:apply-templates select="ownedComment"/>
      <xsl:if test="not(ownedComment)">
        <xsl:call-template name="Comment"/>
      </xsl:if>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedAttribute">
    <xsl:choose>
      <xsl:when test="not(@association)">
        <xsl:call-template name="Property"/>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedOperation">
    <xsl:choose>
      <xsl:when test="@xmi:type='uml:Operation'">
        <xsl:call-template name="Method"/>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedOperation" mode="Constructor">
    <xsl:attribute name="constructor">
      <xsl:value-of select="@visibility"/>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedOperation" mode="Destructor">
    <xsl:attribute name="destructor">
      <xsl:value-of select="@visibility"/>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedParameter">
    <xsl:choose>
      <xsl:when test="@direction='return'">
        <xsl:call-template name="Return"/>
      </xsl:when>
      <xsl:when test="@name!=''">
        <xsl:call-template name="Parameter"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type">
    <xsl:element name="type" use-attribute-sets="Type">
      <xsl:choose>
        <xsl:when test="@xmi:idref">
          <xsl:variable name="Idref" select="@xmi:idref"/>
          <xsl:choose>
            <xsl:when test="//packagedElement[@xmi:type='uml:DataType' and @xmi:id=$Idref]">
              <xsl:attribute name="desc">
                <xsl:value-of select="//packagedElement[@xmi:type='uml:DataType' and @xmi:id=$Idref]/@name"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="idref">
                <xsl:value-of select="$Idref"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="@href">
          <xsl:attribute name="desc">
            <xsl:value-of select="substring-after(@href,'#')"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="idref">
            <xsl:value-of select="INCONNU"/>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Package">
    <xsl:element name="package" use-attribute-sets="Node">
      <xsl:if test="not(ownedComment)">
        <xsl:call-template name="Comment"/>
      </xsl:if>
      <xsl:apply-templates/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Association">
    <xsl:element name="relationship">
      <xsl:attribute name="id">
        <xsl:value-of select="@xmi:id"/>
      </xsl:attribute>
      <xsl:attribute name="action">
        <xsl:value-of select="ownedComment/@body"/>.
      </xsl:attribute>
      <xsl:attribute name="type">assembl</xsl:attribute>
      <xsl:choose>
        <xsl:when test="ownedEnd[@isNavigable='false']">
          <xsl:apply-templates select="ownedEnd[1]" mode="Association">
            <xsl:with-param name="Association" select="@xmi:id"/>
            <xsl:with-param name="Father">no</xsl:with-param>
            <xsl:with-param name="ChildName">father</xsl:with-param>
          </xsl:apply-templates>
          <xsl:apply-templates select="memberEnd[not(@xmi:idref=//ownedEnd/@xmi:id)]" mode="Association">
            <xsl:with-param name="Association" select="@xmi:id"/>
            <xsl:with-param name="Father">yes</xsl:with-param>
            <xsl:with-param name="ChildName">child</xsl:with-param>
          </xsl:apply-templates>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="memberEnd[1]" mode="Association">
            <xsl:with-param name="Association" select="@xmi:id"/>
            <xsl:with-param name="Father">yes</xsl:with-param>
            <xsl:with-param name="ChildName">father</xsl:with-param>
          </xsl:apply-templates>
          <xsl:apply-templates select="memberEnd[2]" mode="Association">
            <xsl:with-param name="Association" select="@xmi:id"/>
            <xsl:with-param name="Father">yes</xsl:with-param>
            <xsl:with-param name="ChildName">child</xsl:with-param>
          </xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="memberEnd | ownedEnd" mode="Association">
    <xsl:param name="Association"/>
    <xsl:param name="Father"/>
    <xsl:param name="ChildName"/>
    <xsl:choose>
      <xsl:when test="$Father='no'">
        <xsl:call-template name="">
          <xsl:with-param name="ChildName">father</xsl:with-param>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy-of select="."/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="NotNavigable">
    <xsl:param name="ChildName"/>
    <xsl:element name="{$ChildName}">
      <xsl:attribute name="name">father</xsl:attribute>
      <xsl:attribute name="range">no</xsl:attribute>
      <xsl:attribute name="idref">
        <xsl:value-of select="type/@xmi:idref"/>
      </xsl:attribute>
      <xsl:attribute name="cardinal">1</xsl:attribute>
      <xsl:element name="get">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="modifier">var</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
      <xsl:element name="set">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedAttribute" mode="Association">
    <xsl:param name="ChildName"/>
    <xsl:element name="{$ChildName}">
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="range">
        <xsl:value-of select="@visibility"/>
      </xsl:attribute>
      <xsl:attribute name="idref">
        <xsl:value-of select="type/@xmi:idref"/>
      </xsl:attribute>
      <xsl:variable name="LowerValue">
        <xsl:choose>
          <xsl:when test="lowerValue[@value='*']">0</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="lowerValue/@value"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:attribute name="cardinal">[
        <xsl:value-of select="$LowerValue"/>|
        <xsl:choose>
          <xsl:when test="upperValue[@value='*']">n</xsl:when>
          <xsl:when test="upperValue[@value='1' and $LowerValue='1']"/>
          <xsl:otherwise>
            <xsl:value-of select="upperValue/@value"/>
          </xsl:otherwise>
        </xsl:choose>]
      </xsl:attribute>
      <xsl:element name="get">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="modifier">var</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
      <xsl:element name="set">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Property">
    <xsl:element name="property">
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="num-id">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="member">object</xsl:attribute>
      <xsl:apply-templates select="type"/>
      <xsl:element name="variable">
        <xsl:attribute name="range">
          <xsl:value-of select="@visibility"/>
        </xsl:attribute>
      </xsl:element>
      <xsl:apply-templates select="ownedComment"/>
      <xsl:if test="not(ownedComment)">
        <xsl:call-template name="Comment"/>
      </xsl:if>
      <xsl:element name="get">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="modifier">var</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
      <xsl:element name="set">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Parameter">
    <xsl:element name="param">
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="num-id">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:apply-templates/>
      <xsl:element name="variable">
        <xsl:attribute name="range">private</xsl:attribute>
      </xsl:element>
      <xsl:apply-templates select="ownedComment"/>
      <xsl:if test="not(ownedComment)">
        <xsl:call-template name="Comment"/>
      </xsl:if>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Return">
    <xsl:element name="return">
      <xsl:apply-templates/>
      <xsl:element name="variable">
        <xsl:attribute name="range">
          <xsl:value-of select="parent::*/@visibility"/>
        </xsl:attribute>
      </xsl:element>
      <xsl:apply-templates select="ownedComment"/>
      <xsl:if test="not(ownedComment)">
        <xsl:call-template name="Comment"/>
      </xsl:if>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Method">
    <xsl:element name="method">
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="num-id">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="constructor">no</xsl:attribute>
      <xsl:attribute name="member">object</xsl:attribute>
      <xsl:call-template name="Implementation"/>
      <xsl:apply-templates select="ownedParameter[@direction='return']"/>
      <xsl:apply-templates select="ownedComment"/>
      <xsl:if test="not(ownedComment)">
        <xsl:call-template name="Comment"/>
      </xsl:if>
      <xsl:apply-templates select="ownedParameter[@direction!='return']"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Class">
    <xsl:variable name="Name" select="@name"/>
    <xsl:element name="class" use-attribute-sets="Node">
      <xsl:copy-of select="@visibility"/>
      <xsl:attribute name="constructor">no</xsl:attribute>
      <xsl:attribute name="destructor">no</xsl:attribute>
      <xsl:apply-templates select="ownedOperation[@name=$Name]" mode="Constructor"/>
      <xsl:apply-templates select="ownedOperation[starts-with(@name,'~')]" mode="Destructor"/>
      <xsl:attribute name="inline">none</xsl:attribute>
      <xsl:call-template name="Implementation"/>
      <xsl:apply-templates select="ownedComment"/>
      <xsl:if test="not(ownedComment)">
        <xsl:call-template name="Comment"/>
      </xsl:if>
      <xsl:apply-templates select="nestedClassifier"/>
      <xsl:apply-templates select="ownedAttribute"/>
      <xsl:apply-templates select="ownedOperation[@name!=$Name and not(starts-with(@name,'~'))]"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Implementation">
    <xsl:attribute name="implementation">
      <xsl:choose>
        <xsl:when test="@isAbstract='true'">abstract</xsl:when>
        <xsl:otherwise>simple</xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
</xsl:stylesheet>

















































































