<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:uml="http://schema.omg.org/spec/UML/2.1"
                xmlns:xmi="http://schema.omg.org/spec/XMI/2.1"
>
<!-- ======================================================================= -->
  <xsl:output method="xml" encoding="ISO-8859-1" media-type="xmi" indent="yes"/><!--doctype-system="xmi2-1-uml2-1.dtd" standalone="no"/-->
  <!-- ======================================================================= -->
  <xsl:param name="LanguageFolder"/>
  <!-- ======================================================================= -->
  <xsl:include href="xmi-types.xsl"/>
  <!-- ======================================================================= -->
  <xsl:template name="GetRoot" match="/root">
    <xmi:XMI xmi:version="2.1">
      <uml:Model xmi:type="uml:Model" xmi:id="themodel" name="{@name}">
        <xsl:apply-templates select="class" mode="Code"/>
        <xsl:apply-templates select="package" mode="Code"/>
        <xsl:apply-templates select="relationship" mode="Code"/>
        <packagedElement xmi:type="uml:Package" xmi:id="{generate-id()}" name="PredefinedTypes">
          <xsl:copy-of select="$PredefinedTypes"/>
        </packagedElement>
      </uml:Model>
    </xmi:XMI>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetIdref" match="@idref | @index-idref | @valref | @sizeref" mode="Code">
    <xsl:value-of select="."/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetPackage" match="package" mode="Code">
    <packagedElement xmi:type="uml:Package" xmi:id="{@id}" name ="{@name}">
      <xsl:apply-templates select="comment"/>
      <xsl:apply-templates select="class" mode="Code"/>
      <xsl:apply-templates select="package" mode="Code"/>
    </packagedElement>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetClass" match="class" mode="Code">
    <packagedElement xmi:type="uml:Class" name="{@name}" xmi:id="{@id}">
      <xsl:attribute name="visibility">
        <xsl:choose>
          <xsl:when test="@visibility='package'">package</xsl:when>
          <xsl:otherwise>public</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="isLeaf">
        <xsl:choose>
          <xsl:when test="@implementation='final'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="isAbstract">
        <xsl:choose>
          <xsl:when test="@implementation='abstract'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates select="comment"/>
      <xsl:apply-templates select="model" mode="Class"/>
      <xsl:apply-templates select="inherited" mode="Code"/>
      <xsl:apply-templates select="property[type/@modifier='const']" mode="Code"/>
      <xsl:apply-templates select="typedef" mode="Code"/>
      <xsl:call-template name="Properties"/>
      <xsl:call-template name="Functions"/>
    </packagedElement>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetModel" match="model" mode="Class">
    <templateParameter xmi:type="uml:TemplateParameter" xmi:label="{@name}" xmi:id="{@id}"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetInherited" match="inherited" mode="Code">
    <generalization xmi:type="uml:Generalization" xmi:id="{generate-id()}" general="{@idref}" specific="{parent::class/@id}"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Properties">
    <xsl:variable name="ClassID" select="@id"/>
    <!-- Properties -->
    <xsl:apply-templates select="property[type/@modifier!='const']" mode="Code"/>
    <xsl:apply-templates select="//relationship/child[preceding-sibling::father/@idref=$ClassID]" mode="Relation">
      <xsl:with-param name="Implementation" select="@implementation"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="//relationship/father[following-sibling::child/@idref=$ClassID]" mode="Relation">
      <xsl:with-param name="Implementation" select="@implementation"/>
    </xsl:apply-templates>
    <!-- Accessors -->
    <xsl:apply-templates select="property[type/@modifier!='const'][get[@range!='no'] or set[@range!='no']]" mode="Access"/>
    <xsl:apply-templates select="//relationship/child[preceding-sibling::father/@idref=$ClassID]" mode="Access">
      <xsl:with-param name="Implementation" select="@implementation"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="//relationship/father[following-sibling::child/@idref=$ClassID]" mode="Access">
      <xsl:with-param name="Implementation" select="@implementation"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetConstructor" match="@constructor">
    <xsl:if test=".!='no'">
      <xsl:choose>
        <xsl:when test="$LanguageValue='0'">
          <ownedOperation xmi:type="uml:Operation" xmi:id="{generate-id()}" name="{parent::class/@name}" visibility="{.}" class="{parent::class/@id}"/>
        </xsl:when>
        <xsl:when test="$LanguageValue='2'">
          <ownedOperation xmi:type="uml:Operation" xmi:id="{generate-id()}" name="{parent::class/@name}" visibility="{.}" class="{parent::class/@id}"/>
        </xsl:when>
        <xsl:otherwise>
          <ownedOperation xmi:type="uml:Operation" xmi:id="{generate-id()}" name="New" visibility="{.}" class="{parent::class/@id}"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetDestructor" match="@destructor">
    <xsl:if test=".!='no'">
      <xsl:choose>
        <xsl:when test="$LanguageValue='0'">
          <ownedOperation xmi:type="uml:Operation" xmi:id="{generate-id()}" name="~{parent::class/@name}" visibility="{.}" class="{parent::class/@id}"/>
        </xsl:when>
        <xsl:when test="$LanguageValue='2'">
          <ownedOperation xmi:type="uml:Operation" xmi:id="{generate-id()}" name="Finalize" visibility="{.}" class="{parent::class/@id}"/>
        </xsl:when>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Functions">
    <xsl:apply-templates select="@constructor"/>
    <xsl:apply-templates select="@destructor"/>
    <xsl:apply-templates select="method" mode="Code"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetTypedef" match="typedef" mode="Code">
    <nestedClassifier name="{@name}" xmi:id="{@id}" visibility="{variable/@range}" isAbstract="false">
      <xsl:attribute name="xmi:type">
        <xsl:choose>
          <xsl:when test="type/enumvalue">uml:Enumeration</xsl:when>
          <xsl:otherwise>uml:Class</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates select="comment"/>
      <xsl:apply-templates select="type" mode="Code">
        <xsl:with-param name="TypedefID" select="@id"/>
      </xsl:apply-templates>
    </nestedClassifier>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetEnumvalue" match="enumvalue" mode="Code">
    <ownedLiteral xmi:type="uml:EnumerationLiteral" xmi:id="{@id}" name="{@name}" enumeration="{ancestor::typedef/@id}">
      <specification xmi:type="uml:LiteralString" xmi:id="{generate-id()}" value="{@value}"/>
      <xsl:call-template name="Comment"/>
    </ownedLiteral>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetElement" match="element" mode="Code">
    <ownedAttribute xmi:type="uml:Property" name="{@name}" xmi:id="{generate-id()}" visibility="public">
      <xsl:attribute name="isReadOnly">
        <xsl:choose>
          <xsl:when test="@modifier='const'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:call-template name="Comment"/>
      <xsl:call-template name="Type">
        <xsl:with-param name="TypedefID" select="generate-id()"/>
      </xsl:call-template>
    </ownedAttribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetMethod" match="method" mode="Code">
    <ownedOperation xmi:type="uml:Operation" xmi:id="{generate-id()}" class="{parent::class/@id}">
      <xsl:attribute name="name">
        <xsl:choose>
          <xsl:when test="@constructor!='no' and $LanguageValue='2'">New</xsl:when>
          <xsl:when test="@constructor!='no' and $LanguageValue!='1'">
            <xsl:value-of select="parent::class/@name"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="@name"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="isLeaf">
        <xsl:choose>
          <xsl:when test="@implementation='final'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="isAbstract">
        <xsl:choose>
          <xsl:when test="@implementation='abstract'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:if test="@overrides">
        <xsl:attribute name="xmi:idref">
          <xsl:value-of select="@overrides"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="isStatic">
        <xsl:choose>
          <xsl:when test="@member='class'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="visibility">
        <xsl:choose>
          <xsl:when test="@constructor!='no'">
            <xsl:value-of select="@constructor"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="return/variable/@range"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates select="comment"/>
      <xsl:apply-templates select="return | param" mode="Code"/>
    </ownedOperation>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Type" match="type" mode="Code">
    <xsl:param name="TypedefID"/>
    <xsl:variable name="TypeID">
      <xsl:choose>
        <xsl:when test="@desc">
          <xsl:apply-templates select="@desc" mode="Code"/>
        </xsl:when>
        <xsl:when test="@idref">
          <xsl:value-of select="@idref"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="list">
        <xsl:apply-templates select="list" mode="Code">
          <xsl:with-param name="TypeID" select="$TypeID"/>
          <xsl:with-param name="TypedefID" select="$TypedefID"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:when test="@desc or @idref">
        <type xmi:id="{$TypedefID}_1" xmi:type="uml:Class">
          <xsl:attribute name="xmi:idref">
            <xsl:value-of select="$TypeID"/>
          </xsl:attribute>
        </type>
      </xsl:when>
      <xsl:when test="enumvalue">
        <xsl:apply-templates select="*" mode="Code"/>
      </xsl:when>
      <xsl:when test="element">
        <xsl:apply-templates select="*" mode="Code"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:comment>TODO</xsl:comment>
        <xsl:copy-of select="*"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetVariable" match="variable" mode="Code">
    <xsl:choose>
      <xsl:when test="@value">
        <defaultValue xmi:type="uml:LiteralSpecification" xmi:id="{generate-id()}_{position()}_1" value="{@value}"/>
      </xsl:when>
      <xsl:when test="@size">
        <upperValue xmi:type="uml:LiteralSpecification" xmi:id="{generate-id()}_{position()}_1" value="{@size}"/>
      </xsl:when>
      <xsl:when test="@valref">
        <defaultValue xmi:type="uml:LiteralSpecification" xmi:id="{generate-id()}_{position()}_1" xmi:idref="{@valref}"/>
      </xsl:when>
      <xsl:when test="@sizeref">
        <upperValue xmi:type="uml:LiteralSpecification" xmi:id="{generate-id()}_{position()}_1" xmi:idref="{@sizeref}"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetParam" match="param" mode="Code">
    <ownedParameter name="{@name}" xmi:id="{generate-id()}">
      <xsl:attribute name="direction">
        <xsl:choose>
          <xsl:when test="@modifier='const'">in</xsl:when>
          <xsl:when test="@by='ref'">inout</xsl:when>
          <xsl:otherwise>in</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates select="comment"/>
      <xsl:apply-templates select="type" mode="Code">
        <xsl:with-param name="TypedefID" select="generate-id()"/>
      </xsl:apply-templates>
      <xsl:apply-templates select="variable" mode="Code"/>
    </ownedParameter>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="return" mode="Code">
    <ownedParameter name="return" xmi:id="{generate-id()}" direction="return">
      <xsl:apply-templates select="comment"/>
      <xsl:apply-templates select="type" mode="Code">
        <xsl:with-param name="TypedefID" select="generate-id()"/>
      </xsl:apply-templates>
    </ownedParameter>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetProperty" match="property" mode="Code">
    <xsl:if test="@attribute='yes'">
      <ownedAttribute xmi:type="uml:Property" name="{@name}" xmi:id="{generate-id()}" visibility="{variable/@range}">
        <xsl:attribute name="isReadOnly">
          <xsl:choose>
            <xsl:when test="type/@modifier='const'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="isStatic">
          <xsl:choose>
            <xsl:when test="@member='class'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:if test="@overrides">
          <xsl:attribute name="xmi:idref">
            <xsl:value-of select="@overrides"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:apply-templates select="comment"/>
        <xsl:apply-templates select="type" mode="Code">
        <xsl:with-param name="TypedefID" select="generate-id()"/>
        </xsl:apply-templates>
        <xsl:apply-templates select="variable" mode="Code"/>
      </ownedAttribute>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetPropertyAccess" match="property" mode="Access">
    <xsl:variable name="Name">
      <xsl:apply-templates select="@name" mode="Accessor"/>
    </xsl:variable>
    <xsl:apply-templates select="get[@range!='no']">
      <xsl:with-param name="Name" select="$Name"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="set[@range!='no']">
      <xsl:with-param name="Name" select="$Name"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child | father" mode="Access">
    <xsl:param name="Implementation"/>
    <xsl:variable name="Id">
      <xsl:choose>
        <xsl:when test="$Implementation='abstract'">
          <xsl:value-of select="parent::*/@id"/>
          <xsl:choose>
            <xsl:when test="self::child">_2</xsl:when>
            <xsl:otherwise>_1</xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="generate-id()"/>
          <xsl:choose>
            <xsl:when test="self::child">_2</xsl:when>
            <xsl:otherwise>_1</xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Name">
      <xsl:apply-templates select="@name" mode="Accessor"/>
    </xsl:variable>
    <xsl:if test="get[@range!='no']">
      <ownedOperation xmi:type="uml:Operation" name="{$GetName}{$Name}" xmi:id="{$Id}" visibility="{get/@range}">
        <xsl:attribute name="isStatic">
          <xsl:choose>
            <xsl:when test="@member='class'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="isAbstract">
          <xsl:choose>
            <xsl:when test="$Implementation='abstract'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="isLeaf">false</xsl:attribute>
        <ownedParameter name="return" xmi:id="{$Id}_1" direction="return">
          <type xmi:id="{$Id}_1_1" xmi:type="uml:Class" xmi:idref="{@idref}"/>
        </ownedParameter>
      </ownedOperation>
    </xsl:if>
    <xsl:if test="set[@range!='no']">
      <ownedOperation xmi:type="uml:Operation" name="{$SetName}{$Name}" xmi:id="{$Id}_s1" visibility="{get/@range}">
        <xsl:attribute name="isStatic">
          <xsl:choose>
            <xsl:when test="@member='class'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="isAbstract">
          <xsl:choose>
            <xsl:when test="$Implementation='abstract'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="isLeaf">false</xsl:attribute>
        <ownedParameter name="return" xmi:id="{$Id}_s1_1" direction="return">
          <type xmi:id="{$Id}_s1_1_1" xmi:type="uml:Class">
            <xsl:attribute name="xmi:idref">
              <xsl:call-template name="Void"/>
            </xsl:attribute>
          </type>
        </ownedParameter>
        <ownedParameter name="{$SetParam}" xmi:id="{$Id}_s1_2" direction="in">
          <type xmi:id="{$Id}_s1_2_1" xmi:type="uml:Class" xmi:idref="{@idref}"/>
        </ownedParameter>
      </ownedOperation>
    </xsl:if>
    <xsl:if test="array"/>
    <xsl:if test="list"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="relationship" mode="Code">
    <packagedElement xmi:type="uml:Association" xmi:id="{@id}" memberEnd="{father/@idref} {child/@idref}">
      <xsl:apply-templates select="child | father" mode="Code"/>
    </packagedElement>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child | father" mode="Code">
    <xsl:variable name="Values">
      <xsl:choose>
        <xsl:when test="@cardinal='01'">
          <lowerValue xmi:type="uml:LiteralInteger" xmi:id="{parent::*/@id}_{generate-id()}_1" value="0"/>
          <upperValue xmi:type="uml:LiteralInteger" xmi:id="{parent::*/@id}_{generate-id()}_2" value="1"/>
        </xsl:when>
        <xsl:when test="@cardinal='1'">
          <upperValue xmi:type="uml:LiteralInteger" xmi:id="{parent::*/@id}_{generate-id()}_2" value="1"/>
        </xsl:when>
        <xsl:when test="@cardinal='1n'">
          <lowerValue xmi:type="uml:LiteralInteger" xmi:id="{parent::*/@id}_{generate-id()}_1" value="1"/>
          <upperValue xmi:type="uml:LiteralUnlimitedNatural" xmi:id="{parent::*/@id}_{generate-id()}_2" value="*"/>
        </xsl:when>
        <xsl:otherwise>
          <lowerValue xmi:type="uml:LiteralInteger" xmi:id="{parent::*/@id}_{generate-id()}_1" value="0"/>
          <upperValue xmi:type="uml:LiteralUnlimitedNatural" xmi:id="{parent::*/@id}_{generate-id()}_2" value="*"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Aggregation">
      <xsl:choose>
        <xsl:when test="parent::*/@type='aggreg'">shared</xsl:when>
        <xsl:when test="parent::*/@type='comp'">composite</xsl:when>
        <xsl:otherwise>none</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Position">
      <xsl:choose>
        <xsl:when test="self::child">2</xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="@range='no'">
        <ownedEnd xmi:type="uml:Property" xmi:id="{generate-id()}" type="{@idref}" association="{parent::*/@id}">
          <xsl:attribute name="aggregation">
            <xsl:value-of select="$Aggregation"/>
          </xsl:attribute>
          <xsl:copy-of select="$Values"/>
        </ownedEnd>
      </xsl:when>
      <xsl:otherwise>
        <memberEnd xmi:type="uml:Property" xmi:id="{generate-id()}" type="{@idref}" xmi:idref="{parent::*/@id}_{$Position}" association="{parent::*/@id}">
          <xsl:attribute name="aggregation">
            <xsl:value-of select="$Aggregation"/>
          </xsl:attribute>
          <xsl:copy-of select="Values"/>
        </memberEnd>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child | father" mode="Relation">
    <xsl:param name="Implementation"/>
    <xsl:if test="$Implementation!='abstract'">
      <xsl:variable name="Position">
        <xsl:choose>
          <xsl:when test="self::child">2</xsl:when>
          <xsl:otherwise>1</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <ownedAttribute xmi:type="uml:Property" name="{@name}" xmi:id="{parent::*/@id}_{$Position}" visibility="{@range}">
        <xsl:attribute name="association">
          <xsl:value-of select="parent::*/@id"/>
        </xsl:attribute>
        <xsl:attribute name="aggregation">
          <xsl:choose>
            <xsl:when test="parent::*/@type='aggreg'">shared</xsl:when>
            <xsl:when test="parent::*/@type='comp'">composite</xsl:when>
            <xsl:otherwise>none</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="isStatic">
          <xsl:choose>
            <xsl:when test="@member='class'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <ownedComment xmi:type="uml:Comment" xmi:id="{generate-id()}_comment" body="{parent::relationship/@action}"/>
        <xsl:call-template name="Type">
          <xsl:with-param name="TypedefID" select="concat(parent::*/@id,'_',$Position)"/>
        </xsl:call-template>
      </ownedAttribute>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@name" mode="Accessor">
    <xsl:value-of select="translate(substring(.,1,1),'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
    <xsl:value-of select="substring(.,2)"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Comment" match="comment">
    <xsl:variable name="Id">
      <xsl:choose>
       <xsl:when test="self::comment"><xsl:value-of select="generate-id()"/></xsl:when>
        <xsl:otherwise><xsl:value-of select="generate-id()"/>_comment</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="string-length(@brief)=0">
        <ownedComment xmi:type="uml:Comment" xmi:id="{$Id}" body="{text()}"/>
      </xsl:when>
      <xsl:when test="string-length(text())=0">
        <ownedComment xmi:type="uml:Comment" xmi:id="{$Id}" body="{@brief}"/>
      </xsl:when>
      <xsl:otherwise>
        <ownedComment xmi:type="uml:Comment" xmi:id="{$Id}" body="{@brief}">
          <ownedComment xmi:type="uml:Comment" xmi:id="{$Id}_1" body="{text()}"/>
        </ownedComment>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetListTemplate" match="list" mode="Template">
    <xsl:param name="Name"/>
    <packagedElement xmi:type="uml:Class" name="{$Name}" xmi:label="{$Name}">
      <templateParameter xmi:type="uml:TemplateParameter" xmi:label="value"/>
      <xsl:if test="@type='indexed'">
        <templateParameter xmi:type="uml:TemplateParameter" xmi:label="key"/>
      </xsl:if>
    </packagedElement>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="list" mode="Code">
    <xsl:param name="TypeID"/>
    <xsl:variable name="ListID">
      <xsl:apply-templates select="(@desc |@idref)" mode="Code"/>
    </xsl:variable>
    <redefinedClassifier xmi:type="uml:Classifier" xmi:id="{$ListID}_{generate-id()}" xmi:idref="{$ListID}">
      <xsl:call-template name="ImplementTemplate">
        <xsl:with-param name="ListID" select="$ListID"/>
        <xsl:with-param name="Parameter">value</xsl:with-param>
          <xsl:with-param name="Idref" select="$TypeID"/>
      </xsl:call-template>
      <!---templateParameter xmi:type="uml:TemplateParameter" xmi:id="{$TypeID}_1_1" xmi:idref="{$ListID}_1">
        <parameteredElement xmi:type="uml:ParameterableElement" xmi:id="{$TypeID}_1_1_1" xmi:idref="{$TypeID}"/>
      </templateParameter-->
      <xsl:if test="@type='indexed'">
        <xsl:variable name="IndexID">
          <xsl:apply-templates select="(@index-desc |@index-idref)" mode="Code"/>
        </xsl:variable>
        <xsl:call-template name="ImplementTemplate">
          <xsl:with-param name="ListID" select="$ListID"/>
          <xsl:with-param name="Parameter">key</xsl:with-param>
          <xsl:with-param name="Idref" select="$IndexID"/>
        </xsl:call-template>
        <!--templateParameter xmi:type="uml:TemplateParameter" xmi:id="{$TypeID}_1_2" xmi:idref="{$ListID}_2">
          <parameteredElement xmi:type="uml:ParameterableElement" xmi:id="{$TypeID}_1_2_1" xmi:idref="{$IndexID}"/>
        </templateParameter-->
      </xsl:if>
    </redefinedClassifier>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="get" mode="Code">
    <xsl:param name="Name"/>
    <ownedOperation xmi:type="uml:Operation" name="{$GetName}{$Name}" xmi:id="{generate-id()}" visibility="{get/@range}">
      <xsl:attribute name="isStatic">
        <xsl:choose>
          <xsl:when test="@member='class'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="isAbstract">
        <xsl:choose>
          <xsl:when test="@attribute='no' and @overridable='yes'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="isLeaf">
        <xsl:choose>
          <xsl:when test="(@overrides) and @overridable='no'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <ownedParameter name="return" xmi:id="{generate-id()}_1" direction="return">
        <xsl:apply-templates select="type" mode="Code">
          <xsl:with-param name="TypedefID" select="concat(generate-id(),'_1')"/>
        </xsl:apply-templates>
      </ownedParameter>
    </ownedOperation>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="set" mode="Code">
    <xsl:param name="Name"/>
    <ownedOperation xmi:type="uml:Operation" name="{$SetName}{$Name}" xmi:id="{generate-id()}" visibility="{get/@range}">
      <xsl:attribute name="isStatic">
        <xsl:choose>
          <xsl:when test="@member='class'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="isAbstract">
        <xsl:choose>
          <xsl:when test="@attribute='no' and @overridable='yes'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="isLeaf">false</xsl:attribute>
      <ownedParameter name="return" xmi:id="{generate-id()}_1" direction="return">
        <type xmi:type="uml:Class">
          <xsl:attribute name="xmi:idref">
            <xsl:call-template name="Void"/>
          </xsl:attribute>
        </type>
      </ownedParameter>
      <ownedParameter name="{$SetParam}" xmi:id="{generate-id()}_2" direction="in">
        <xsl:apply-templates select="type" mode="Code">
          <xsl:with-param name="TypedefID" select="concat(generate-id(),'_2')"/>
        </xsl:apply-templates>
      </ownedParameter>
    </ownedOperation>
  </xsl:template>
</xsl:stylesheet>
































