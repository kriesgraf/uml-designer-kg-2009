<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:uml="http://schema.omg.org/spec/UML/2.1"
                xmlns:xmi="http://schema.omg.org/spec/XMI/2.1"
>
<!-- ======================================================================= -->
  <xsl:output method="xml" encoding="ISO-8859-1" media-type="xmi" indent="yes"/>
  <!--xsl:output method="xml" encoding="ISO-8859-1" media-type="xmi" indent="yes" doctype-system="xmi2-1-uml2-1.dtd" standalone="no"/-->
  <!-- ======================================================================= -->
  <xsl:param name="LanguageFolder"/>
  <!-- ======================================================================= -->
  <xsl:include href="xmi-types.xsl"/>
  <!-- ======================================================================= -->
  <xsl:key match="@package" name="package" use="."/>
  <!-- ======================================================================= -->
  <xsl:template name="GetRoot" match="/root">
    <xmi:XMI xmi:version="2.1">
      <uml:Model xmi:type="uml:Model" xmi:id="themodel" name="{@name}">
        <xsl:apply-templates select="class" mode="Code"/>
        <xsl:apply-templates select="package" mode="Code"/>
        <xsl:apply-templates select="relationship" mode="Code"/>
        <!--  We separate interface and reference because we must maintain UIDs for reference solve -->
        <xsl:apply-templates select="//import" mode="ImplementImports"/>
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
  <xsl:template name="GetTypePredefinedTypes" match="type" mode="PredefinedTypes">
    <packagedElement xmi:type="uml:DataType" name="{@name}" href="{@implementation}"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="import" mode="ImplementImports">
    <xsl:choose>
      <xsl:when test="string-length(@param)!=0">
        <packagedElement xmi:type="uml:Package" name="{@param}" xmi:id="{generate-id()}">
          <xsl:apply-templates select="." mode="PredefinedTypes"/>
        </packagedElement>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="." mode="PredefinedTypes"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetImportImplementImports" match="import" mode="PredefinedTypes">
    <xsl:for-each select="export/*[@package='' or not(@package)]">
      <xsl:apply-templates select="." mode="PredefinedTypes"/>
    </xsl:for-each>
    <xsl:for-each select="export/*/@package[generate-id()=generate-id(key('package',.)[1])]">
      <xsl:sort select="."/>
      <xsl:variable name="Current" select="."/>
      <packagedElement xmi:type="uml:Package" name="{.}" xmi:id="{generate-id()}">
      <xsl:for-each select="ancestor::export/*[@package=$Current]">
        <xsl:apply-templates select="." mode="PredefinedTypes"/>
      </xsl:for-each>
      </packagedElement>
    </xsl:for-each>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="interface" mode="PredefinedTypes">
    <packagedElement xmi:type="uml:Class" name="{@name}" xmi:id="{@id}">
      <xsl:apply-templates select="property | method" mode="Code"/>
    </packagedElement>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference" mode="PredefinedTypes">
    <packagedElement xmi:type="uml:Class" name="{@name}" xmi:id="{@id}">
      <xsl:if test="@container='1' or @container='2'">
        <xsl:variable name="ClassID" select="@id"/>
        <xsl:variable name="Signature">_sign</xsl:variable>
        <xsl:variable name="ValueId" select="concat($ClassID, '_1', $Signature)"/>
        <xsl:variable name="KeyId" select="concat($ClassID, '_2', $Signature)"/>
        <xsl:variable name="ModelIds">
          <xsl:value-of select="$ValueId"/>
          <xsl:if test="@container='2'">
            <xsl:value-of select="concat(' ', $KeyId)"/>
          </xsl:if>
        </xsl:variable>
        <ownedTemplateSignature xmi:type="uml:RedefinableTemplateSignature" xmi:id="{$ClassID}{$Signature}" parameter="{$ModelIds}" template="{$ClassID}">
          <ownedParameter xmi:type="uml:ClassifierTemplateParameter" xmi:id="{$ValueId}" signature="{$ClassID}{$Signature}" parameteredElement="{$ValueId}_1">
            <ownedParameteredElement xmi:type="uml:DataType" xmi:id="{$ValueId}_1" name="value" owningTemplateParameter="{$ValueId}" templateParameter="{$ValueId}"/>
          </ownedParameter>
          <xsl:if test="@container='2'">
            <ownedParameter xmi:type="uml:ClassifierTemplateParameter" xmi:id="{$KeyId}" signature="{$ClassID}{$Signature}" parameteredElement="{$KeyId}_1">
              <ownedParameteredElement xmi:type="uml:DataType" xmi:id="{$KeyId}_1" name="key" owningTemplateParameter="{$KeyId}" templateParameter="{$KeyId}"/>
            </ownedParameter>
          </xsl:if>
        </ownedTemplateSignature>
        <nestedClassifier xmi:type="uml:Class" xmi:id="{$ValueId}_1_1" name="value"/>
        <xsl:if test="@container='2'">
          <nestedClassifier xmi:type="uml:Class" xmi:id="{$KeyId}_1_1" name="key"/>
        </xsl:if>
      </xsl:if>
    </packagedElement>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetClass" match="class" mode="Code">
    <xsl:variable name="ClassID">
      <xsl:value-of select="@id"/>
    </xsl:variable>
    <xsl:variable name="Type">
      <xsl:choose>
        <xsl:when test="@implementation='abstract'">Interface</xsl:when>
        <xsl:otherwise>Class</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <packagedElement xmi:type="uml:{$Type}" name="{@name}" xmi:id="{$ClassID}">
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
      <xsl:if test="model">
        <xsl:apply-templates select="." mode="TemplateSignature"/>
        <xsl:for-each select="model">
          <nestedClassifier xmi:type="uml:Class" xmi:id="{@id}" name="{@name}"/>
        </xsl:for-each>
      </xsl:if>
      <xsl:apply-templates select="model" mode="Class"/>
      <xsl:apply-templates select="inherited" mode="Code"/>
      <xsl:apply-templates select="property[type/@modifier='const']" mode="Code"/>
      <xsl:apply-templates select="typedef" mode="Code"/>
      <xsl:call-template name="Properties"/>
      <xsl:call-template name="Functions"/>
    </packagedElement>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetInherited" match="inherited" mode="Code">
    <xsl:choose>
      <xsl:when test="id(@idref)/@implementation='abstract'">
        <interfaceRealization xmi:type="uml:InterfaceRealization" xmi:id="{generate-id()}" name="{parent::class/@name}"
                              supplier="{@idref}" client="{parent::class/@id}"
                              contract="{@idref}" implementingClassifier="{parent::class/@id}"/>
      </xsl:when>
      <xsl:otherwise>
        <generalization xmi:type="uml:Generalization" xmi:id="{generate-id()}" general="{@idref}" specific="{parent::class/@id}"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetCollaboration" match="collaboration" mode="Code">
    <xsl:variable name="ClassID" select="parent::*/@id"/>
    <xsl:variable name="RefID" select="@idref"/>
    <xsl:variable name="Implementation">
      <xsl:choose>
        <xsl:when test="parent::class">
          <xsl:value-of select="@implementation"/>
        </xsl:when>
        <xsl:when test="parent::reference">simple</xsl:when>
        <xsl:when test="parent::interface">
          <xsl:choose>
            <xsl:when test="@root='yes'">root</xsl:when>
            <xsl:otherwise>abstract</xsl:otherwise>
          </xsl:choose>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:apply-templates select="id($RefID)" mode="Property">
      <xsl:with-param name="ClassID" select="$ClassID"/>
      <xsl:with-param name="Implementation" select="$Implementation"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="id($RefID)" mode="Access">
      <xsl:with-param name="ClassID" select="$ClassID"/>
      <xsl:with-param name="Implementation" select="$Implementation"/>
    </xsl:apply-templates>
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
    <xsl:param name="ParentId"/>
    <nestedClassifier name="{@name}" xmi:id="{@id}{$ParentId}" visibility="{variable/@range}" isAbstract="false">
      <xsl:attribute name="xmi:type">
        <xsl:choose>
          <xsl:when test="type/enumvalue">uml:Enumeration</xsl:when>
          <xsl:otherwise>uml:Class</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="(type/@desc or type/@idref) and not(type/list)">
          <xsl:apply-templates select="comment"/>
          <xsl:apply-templates select="type" mode="RedefinedClassifier">
            <xsl:with-param name="TypedefID" select="@id"/>
          </xsl:apply-templates>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="comment">
            <xsl:with-param name="TypedefID" select="$ParentId"/>
          </xsl:apply-templates>
          <xsl:apply-templates select="type" mode="Code">
            <xsl:with-param name="TypedefID" select="@id"/>
          </xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>
    </nestedClassifier>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetEnumvalue" match="enumvalue" mode="Code">
    <ownedLiteral xmi:type="uml:EnumerationLiteral" xmi:id="{@id}" name="{@name}">
      <xsl:if test="ancestor::typedef">
        <xsl:attribute name="enumeration">
          <xsl:value-of select="ancestor::typedef/@id"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:choose>
        <xsl:when test="@value='' or not(@value)">
          <specification xmi:type="uml:LiteralString" xmi:id="{@id}_spec" value="{@name}"/>
        </xsl:when>
        <xsl:otherwise>
          <specification xmi:type="uml:LiteralString" xmi:id="{@id}_spec" value="{@value}"/>
        </xsl:otherwise>
      </xsl:choose>
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
    <ownedOperation xmi:type="uml:Operation" xmi:id="{generate-id()}">
      <xsl:choose>
        <xsl:when test="parent::class/@implementation='abstract'">
          <xsl:attribute name="interface">
            <xsl:value-of select="parent::*/@id"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="parent::interface/@root='no'">
          <xsl:attribute name="interface">
            <xsl:value-of select="parent::*/@id"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="class">
            <xsl:value-of select="parent::*/@id"/>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
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
      <!--xsl:if test="@overrides">
        <xsl:attribute name="xmi:idref">
          <xsl:value-of select="@overrides"/>
        </xsl:attribute>
      </xsl:if-->
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
      <!--xsl:apply-templates select="comment"/-->
      <xsl:apply-templates select="param" mode="Code"/>
      <xsl:apply-templates select="return" mode="Code"/>
    </ownedOperation>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="TypeRedefinedClassifier" match="type" mode="RedefinedClassifier">
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
    <redefinedClassifier xmi:id="{$TypedefID}_1" xmi:type="uml:Class" xmi:idref="{$TypeID}"/>
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
        <xsl:apply-templates select="list" mode="ImplementTemplate">
          <xsl:with-param name="TypeID" select="$TypeID"/>
          <xsl:with-param name="TypedefID" select="$TypedefID"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:when test="@desc">
        <type xmi:id="{$TypedefID}_1" xmi:type="uml:PrimitiveType" xmi:idref="{$TypeID}"/>
      </xsl:when>
      <xsl:when test="@idref">
        <type xmi:id="{$TypedefID}_1" xmi:type="uml:Class" xmi:idref="{$TypeID}"/>
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
        <defaultValue xmi:type="uml:LiteralString" xmi:id="{generate-id()}_{position()}_1" value="{@value}"/>
      </xsl:when>
      <xsl:when test="@size">
        <lowerValue xmi:type="uml:LiteralString" xmi:id="{generate-id()}_{position()}_1" value="{@size}"/>
        <upperValue xmi:type="uml:LiteralString" xmi:id="{generate-id()}_{position()}_2" value="{@size}"/>
      </xsl:when>
      <xsl:when test="@valref">
        <defaultValue xmi:type="uml:LiteralString" xmi:id="{generate-id()}_{position()}_1" xmi:idref="{@valref}_spec"/>
      </xsl:when>
      <xsl:when test="@sizeref">
        <lowerValue xmi:type="uml:LiteralString" xmi:id="{generate-id()}_{position()}_1" xmi:idref="{@sizeref}_spec"/>
        <upperValue xmi:type="uml:LiteralString" xmi:id="{generate-id()}_{position()}_2" xmi:idref="{@sizeref}_spec"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetParam" match="param" mode="Code">
    <ownedParameter xmi:type="uml:Parameter" name="{@name}" xmi:id="{generate-id()}">
      <xsl:attribute name="direction">
        <xsl:choose>
          <xsl:when test="@modifier='const'">in</xsl:when>
          <xsl:when test="@by='ref'">inout</xsl:when>
          <xsl:otherwise>in</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="type">
        <xsl:apply-templates select="type/@desc | type/@idref" mode="Code"/>
      </xsl:attribute>
      <!--xsl:choose>
        <xsl:when test="type/@desc">
          <xsl:apply-templates select="comment"/>
          <xsl:apply-templates select="type" mode="Code">
            <xsl:with-param name="TypedefID" select="generate-id()"/>
          </xsl:apply-templates>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="comment"/>
          <xsl:apply-templates select="type" mode="Code">
            <xsl:with-param name="TypedefID" select="generate-id()"/>
          </xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose-->
      <!--xsl:apply-templates select="variable" mode="Code"/-->
    </ownedParameter>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetReturn" match="return" mode="Code">
    <ownedParameter xmi:type="uml:Parameter" name="return" xmi:id="{generate-id()}" direction="return">
      <xsl:attribute name="type">
        <xsl:apply-templates select="type/@desc | type/@idref" mode="Code"/>
      </xsl:attribute>
      <!--xsl:apply-templates select="comment"/-->
      <!--xsl:apply-templates select="type" mode="Code">
        <xsl:with-param name="TypedefID" select="generate-id()"/>
      </xsl:apply-templates-->
    </ownedParameter>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetProperty" match="property" mode="Code">
    <xsl:if test="@attribute='yes'">
      <ownedAttribute name="{@name}" xmi:type="uml:Property" xmi:id="{generate-id()}" visibility="{variable/@range}">
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
        <xsl:choose>
          <xsl:when test="type/enumvalue">
            <type xmi:id="{generate-id()}_type" xmi:type="uml:Class" xmi:idref="{generate-id()}_ncf"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="type" mode="Code">
              <xsl:with-param name="TypedefID" select="generate-id()"/>
            </xsl:apply-templates>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:apply-templates select="variable" mode="Code"/>
      </ownedAttribute>
      <xsl:if test="type/enumvalue">
        <xsl:call-template  name="GetTypedef">
          <xsl:with-param name="ParentId" select="concat(generate-id(),'_ncf')"/>
        </xsl:call-template>
      </xsl:if>
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
  <xsl:template match="relationship" mode="Property">
    <xsl:param name="ClassID"/>
    <xsl:param name="Implementation"/>
    <xsl:choose>
      <xsl:when test="not(*[@idref!=$ClassID])">
        <xsl:apply-templates select="*" mode="Relation">
          <xsl:with-param name="Implementation" select="$Implementation"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="*[@idref!=$ClassID]" mode="Relation">
          <xsl:with-param name="Implementation" select="$Implementation"/>
        </xsl:apply-templates>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="relationship" mode="Access">
    <xsl:param name="ClassID"/>
    <xsl:param name="Implementation"/>
    <xsl:choose>
      <xsl:when test="not(*[@idref!=$ClassID])">
        <xsl:apply-templates select="*" mode="Access">
          <xsl:with-param name="Implementation" select="$Implementation"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="*[@idref!=$ClassID]" mode="Access">
          <xsl:with-param name="Implementation" select="$Implementation"/>
        </xsl:apply-templates>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetParentAccess" match="child | father" mode="Access">
    <xsl:param name="Implementation"/>
    <xsl:variable name="Id">
      <xsl:choose>
        <xsl:when test="$Implementation='abstract'">
          <xsl:value-of select="parent::*/@id"/>
          <xsl:choose>
            <xsl:when test="self::child">_2_p</xsl:when>
            <xsl:otherwise>_1_p</xsl:otherwise>
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
        <ownedComment xmi:type="uml:Comment" xmi:id="{$Id}_comment" body="Get relation to class {id(@idref)/@name}"/>
        <ownedParameter xmi:type="uml:Parameter" name="return" xmi:id="{$Id}_1" direction="return">
          <xsl:attribute name="type">
            <xsl:apply-templates select="@idref" mode="Code"/>
          </xsl:attribute>
          <ownedComment xmi:type="uml:Comment" xmi:id="{$Id}_1_comment" body="{id(@idref)/@name} instance"/>
          <!--type xmi:id="{$Id}_1_1" xmi:type="uml:Class" xmi:idref="{@idref}"/-->
        </ownedParameter>
      </ownedOperation>
    </xsl:if>
    <xsl:if test="set[@range!='no']">
      <ownedOperation xmi:type="uml:Operation" name="{$SetName}{$Name}" xmi:id="{$Id}_s1" visibility="{set/@range}">
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
        <ownedComment xmi:type="uml:Comment" xmi:id="{$Id}_s1_comment" body="Set relation to class {id(@idref)/@name}"/>
        <ownedParameter xmi:type="uml:Parameter" name="return" xmi:id="{$Id}_s1_1" direction="return">
          <xsl:attribute name="type">
            <xsl:call-template name="Void"/>
          </xsl:attribute>
        </ownedParameter>
        <ownedParameter xmi:type="uml:Parameter" name="{$SetParam}" xmi:id="{$Id}_s1_2" direction="in">
          <ownedComment xmi:type="uml:Comment" xmi:id="{$Id}_s1_2_comment" body="{id(@idref)/@name} instance"/>
          <type xmi:id="{$Id}_s1_2_1" xmi:type="uml:Class" xmi:idref="{@idref}"/>
        </ownedParameter>
      </ownedOperation>
    </xsl:if>
    <xsl:if test="array"/>
    <xsl:if test="list"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetRelation" match="relationship" mode="Code">
    <packagedElement xmi:type="uml:Association" xmi:id="{@id}" memberEnd="{@id}_1 {@id}_2">
      <xsl:apply-templates select="child | father" mode="Code"/>
    </packagedElement>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetParentCardinal" match="child | father" mode="Code">
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
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetParentCode" match="child | father" mode="Code">
    <xsl:variable name="Values">
      <xsl:call-template name="GetParentCardinal"/>
    </xsl:variable>
    <xsl:variable name="Aggregation">
      <xsl:choose>
        <xsl:when test="parent::*/@type='aggreg'">shared</xsl:when>
        <xsl:when test="parent::*/@type='comp'">composite</xsl:when>
        <xsl:otherwise>none</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Implementation">
      <xsl:choose>
        <xsl:when test="self::child">
          <xsl:choose>
            <xsl:when test="id(parent::*/father/@idref)[self::class]">
              <xsl:value-of select="id(parent::*/father/@idref)/@implementation"/>
            </xsl:when>
            <xsl:otherwise>abstract</xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="id(parent::*/child/@idref)[self::class]">
              <xsl:value-of select="id(parent::class/*/@idref)/@implementation"/>
            </xsl:when>
            <xsl:otherwise>abstract</xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Position">
      <xsl:choose>
        <xsl:when test="self::child">2</xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="@range='no' or $Implementation='abstract'">
        <ownedEnd xmi:type="uml:Property" xmi:id="{parent::*/@id}_{$Position}" type="{@idref}" association="{parent::*/@id}">
          <xsl:attribute name="aggregation">
            <xsl:value-of select="$Aggregation"/>
          </xsl:attribute>
          <xsl:copy-of select="$Values"/>
        </ownedEnd>
      </xsl:when>
      <xsl:otherwise>
        <!--memberEnd xmi:type="uml:Property" xmi:id="{generate-id()}_{position()}" type="{@idref}" association="{parent::*/@id}">
          <xsl:attribute name="aggregation">
            <xsl:value-of select="$Aggregation"/>
          </xsl:attribute>
        </memberEnd-->
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="GetParentRelation" match="child | father" mode="Relation">
    <xsl:param name="Implementation"/>
    <xsl:if test="$Implementation!='abstract' and @range!='no'">
      <xsl:variable name="Position">
        <xsl:choose>
          <xsl:when test="self::child">2</xsl:when>
          <xsl:otherwise>1</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <ownedAttribute xmi:type="uml:Property" name="{@name}" xmi:id="{concat(parent::*/@id,'_',$Position)}" visibility="{@range}">
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
    <xsl:param name="TypedefID"/>
    <xsl:variable name="Id">
      <xsl:choose>
        <xsl:when test="$TypedefID!=''">
          <xsl:value-of select="concat($TypedefID,'_com')"/>
        </xsl:when>
        <xsl:when test="self::comment">
          <xsl:value-of select="generate-id()"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="generate-id()"/>
          <xsl:text>_comment</xsl:text>
        </xsl:otherwise>
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
    <xsl:variable name="ClassID">
      <xsl:value-of select="generate-id()"/>
    </xsl:variable>
    <xsl:variable name="Signature">_sign</xsl:variable>
    <xsl:variable name="ValueId" select="concat($ClassID, '_1', $Signature)"/>
    <xsl:variable name="KeyId" select="concat($ClassID, '_2', $Signature)"/>
    <xsl:variable name="ModelIds">
      <xsl:value-of select="$ValueId"/>
      <xsl:if test="@type='indexed'">
        <xsl:value-of select="concat(' ', $KeyId)"/>
      </xsl:if>
    </xsl:variable>
    <packagedElement xmi:type="uml:Class" name="{$Name}" xmi:id="{$ClassID}" visibility="package" isLeaf="false" isAbstract="false">
      <ownedTemplateSignature xmi:type="uml:RedefinableTemplateSignature" xmi:id="{$ClassID}{$Signature}" parameter="{$ModelIds}" template="{$ClassID}">
        <ownedParameter xmi:type="uml:ClassifierTemplateParameter" xmi:id="{$ValueId}" signature="{$ClassID}{$Signature}" parameteredElement="{$ValueId}_1">
          <ownedParameteredElement xmi:type="uml:DataType" xmi:id="{$ValueId}_1" name="value" owningTemplateParameter="{$ValueId}" templateParameter="{$ValueId}"/>
        </ownedParameter>
        <xsl:if test="@type='indexed'">
          <ownedParameter xmi:type="uml:ClassifierTemplateParameter" xmi:id="{$KeyId}" signature="{$ClassID}{$Signature}" parameteredElement="{$KeyId}_1">
            <ownedParameteredElement xmi:type="uml:DataType" xmi:id="{$KeyId}_1" name="key" owningTemplateParameter="{$KeyId}" templateParameter="{$KeyId}"/>
          </ownedParameter>
        </xsl:if>
      </ownedTemplateSignature>
      <nestedClassifier xmi:type="uml:Class" xmi:id="{$ValueId}_1_1" name="value"/>
      <xsl:if test="@type='indexed'">
        <nestedClassifier xmi:type="uml:Class" xmi:id="{$KeyId}_1_1" name="key"/>
      </xsl:if>
    </packagedElement>
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
      <ownedParameter xmi:type="uml:Parameter" name="return" xmi:id="{generate-id()}_1" direction="return">
        <xsl:attribute name="type">
          <xsl:apply-templates select="type/@desc | type/@idref" mode="Code"/>
        </xsl:attribute>
        <!--xsl:apply-templates select="type" mode="Code">
          <xsl:with-param name="TypedefID" select="concat(generate-id(),'_1')"/>
        </xsl:apply-templates-->
      </ownedParameter>
    </ownedOperation>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="model" mode="ImplementTemplateParameter">
    <xsl:param name="Idref"/>
    <templateParameter xmi:type="uml:TemplateParameter" xmi:idref="{@id}" name="{@name}">
      <parameteredElement xmi:type="uml:ParameterableElement" xmi:id="{generate-id()}_{$Idref}" xmi:idref="{$Idref}"/>
    </templateParameter>
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
      <ownedParameter xmi:type="uml:Parameter" name="return" xmi:id="{generate-id()}_1" direction="return">
        <xsl:attribute name="type">
          <xsl:apply-templates select="type/@desc | type/@idref" mode="Code"/>
        </xsl:attribute>
        <!--type xmi:type="uml:Class">
          <xsl:attribute name="xmi:idref">
            <xsl:call-template name="Void"/>
          </xsl:attribute>
        </type-->
      </ownedParameter>
      <ownedParameter xmi:type="uml:Parameter" name="{$SetParam}" xmi:id="{generate-id()}_2" direction="in">
        <xsl:apply-templates select="type" mode="Code">
          <xsl:with-param name="TypedefID" select="concat(generate-id(),'_2')"/>
        </xsl:apply-templates>
      </ownedParameter>
    </ownedOperation>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="TemplateBinding">
    <xsl:param name="Index"/>
    <xsl:param name="TypeID"/>
    <xsl:param name="ValueID"/>
    <parameterSubstitution xmi:type="uml:TemplateParameterSubstitution" xmi:id="{generate-id()}_tmpl_{$Index}"
                             formal="{$ValueID}" actual="{$TypeID}" templateBinding="{generate-id()}_tmpl"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="TemplateSignature">
    <xsl:variable name="ClassID" select="@id"/>
    <xsl:variable name="Signature">_sign</xsl:variable>
    <xsl:variable name="ModelIds">
      <xsl:for-each select="model">
        <xsl:value-of select="concat(@id, $Signature, ' ')"/>
      </xsl:for-each>
    </xsl:variable>
    <ownedTemplateSignature xmi:type="uml:RedefinableTemplateSignature" xmi:id="{$ClassID}{$Signature}" parameter="{$ModelIds}" template="{$ClassID}">
      <xsl:for-each select="model">
        <ownedParameter xmi:type="uml:ClassifierTemplateParameter" xmi:id="{@id}{$Signature}" signature="{$ClassID}_sign" parameteredElement="{generate-id()}">
          <ownedParameteredElement xmi:type="uml:DataType" xmi:id="{generate-id()}" name="{@name}" owningTemplateParameter="{@id}_sign" templateParameter="{@id}{$Signature}"/>
        </ownedParameter>
      </xsl:for-each>
    </ownedTemplateSignature>
  </xsl:template>
  <!-- ======================================================================= -->
  </xsl:stylesheet>



















































