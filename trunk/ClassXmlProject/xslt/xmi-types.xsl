<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:uml="http://schema.omg.org/spec/UML/2.1"
                xmlns:xmi="http://schema.omg.org/spec/XMI/2.1"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
>
  <!-- ======================================================================= -->
  <xsl:variable name="LanguageValue" select="//generation/@language"/>
  <!-- ======================================================================= -->
  <xsl:variable name="LanguageFile">
    <xsl:value-of select="$LanguageFolder"/>
    <xsl:choose>
      <xsl:when test="$LanguageValue='0'">\LanguageCplusPlus.xml</xsl:when>
      <xsl:when test="$LanguageValue='1'">\LanguageVbasic.xml</xsl:when>
      <xsl:otherwise>\LanguageJava.xml</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name="PredefinedTypes2">
    <xsl:apply-templates select="document($LanguageFile)//type" mode="PredefinedTypes"/>
    <xsl:variable name="List">
      <xsl:for-each select="document($LanguageFile)//type">
        <xsl:value-of select="concat(@name,';',@implementation,';')"/>
      </xsl:for-each>
    </xsl:variable>
    <xsl:for-each select="(//@desc | //@index-desc)">
      <xsl:if test="not(contains($List,.))">
        <xsl:choose>
          <xsl:when test="parent::list">
            <xsl:apply-templates select="parent::list" mode="Template">
              <xsl:with-param name="Name" select="concat(.,id(parent::list/@idref)/@name)"/>
            </xsl:apply-templates>
          </xsl:when>
          <xsl:otherwise>
            <packagedElement xmi:type="uml:DataType" name="{.}" xmi:label="{.}"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>
    </xsl:for-each>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="PredefinedTypes">
    <xsl:for-each select="msxsl:node-set($PredefinedTypes2)/*">
      <xsl:copy>
        <xsl:copy-of select="@*"/>
        <xsl:attribute name="xmi:id"><xsl:value-of select="generate-id()"/></xsl:attribute>
        <xsl:for-each select="*">
          <xsl:copy>
            <xsl:copy-of select="@*"/>
            <xsl:attribute name="xmi:id"><xsl:value-of select="generate-id()"/></xsl:attribute>
          </xsl:copy>
        </xsl:for-each>
      </xsl:copy>
    </xsl:for-each>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:template match="type" mode="PredefinedTypes">
    <xsl:element name="packagedElement">
      <xsl:attribute name="xmi:type">uml:DataType</xsl:attribute>
      <xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute>
      <xsl:attribute name="xmi:label"><xsl:value-of select="@implementation"/></xsl:attribute>
      <xsl:if test="@import">
        <xsl:element name="packageImport">
          <xsl:attribute name="xmi:type">uml:PackageImport</xsl:attribute>
          <xsl:attribute name="xmi:label"><xsl:value-of select="@import"/></xsl:attribute>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:variable name="FileLanguage">
    <xsl:value-of select="$LanguageFolder"/>\language.xml
  </xsl:variable>
  <xsl:variable name="GetName" select="translate(document($FileLanguage)//GetName/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="SetName" select="translate(document($FileLanguage)//SetName/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixList" select="translate(document($FileLanguage)//PrefixList/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixTypeList" select="translate(document($FileLanguage)//PrefixTypeList/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixStructProperty" select="translate(document($FileLanguage)//PrefixStructProperty/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixEnumProperty" select="translate(document($FileLanguage)//PrefixEnumProperty/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="SuffixIterator" select="translate(document($FileLanguage)//SuffixIterator/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixMember" select="translate(document($FileLanguage)//PrefixMember/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixArray" select="translate(document($FileLanguage)//PrefixArray/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="SetParam" select="translate(document($FileLanguage)//SetParam/text(),'&#32;&#10;&#13;','')"/>
  <!-- ======================================================================= -->
  <xsl:template name="Description" match="@desc | @index-desc" mode="Code">
    <xsl:variable name="Description" select="."/>
    <xsl:value-of select="msxsl:node-set($PredefinedTypes)//*[@name=$Description]/@xmi:id"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Void">
    <xsl:value-of select="msxsl:node-set($PredefinedTypes)//*[@name='void']/@xmi:id"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="ImplementTemplate">
    <xsl:param name="ListID"/>
    <xsl:param name="Parameter"/>
    <xsl:param name="Idref"/>
    <xsl:choose>
      <xsl:when test="id($ListID)[self::class]">
        <xsl:variable name="Position">
          <xsl:choose>
            <xsl:when test="$Parameter='value'">A</xsl:when>
            <xsl:when test="$Parameter='key'">B</xsl:when>
            <xsl:otherwise><xsl:value-of select="$Parameter"/></xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:if test="not(id($ListID)/model[@name=$Position])">
          <xsl:message terminate="yes">Can't find for "<xsl:value-of select="$ListID"/>" container parameter "<xsl:value-of select="$Parameter"/>"</xsl:message>
        </xsl:if>
        <xsl:apply-templates select="id($ListID)/model[@name=$Position]" mode="ImplementTemplateParameter">
          <xsl:with-param name="Idref" select="$Idref"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:when test="msxsl:node-set($PredefinedTypes)//packagedElement[@xmi:id=$ListID]">
        <xsl:apply-templates select="msxsl:node-set($PredefinedTypes)//packagedElement[@xmi:id=$ListID]" mode="ImplementTemplateParameter">
          <xsl:with-param name="Parameter" select="$Parameter"/>
          <xsl:with-param name="Idref" select="$Idref"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:message terminate="no">Can't find container "<xsl:value-of select="$ListID"/>"</xsl:message>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="model" mode="ImplementTemplateParameter">
  <xsl:param name="Idref"/>
    <xsl:element name="templateParameter">
      <xsl:attribute name="xmi:type">uml:TemplateParameter</xsl:attribute>
      <xsl:attribute name="xmi:idref"><xsl:value-of select="@id"/></xsl:attribute>
      <xsl:attribute name="xmi:label"><xsl:value-of select="@name"/></xsl:attribute>
      <xsl:element name="parameteredElement">
        <xsl:attribute name="xmi:type">uml:ParameterableElement</xsl:attribute>
        <xsl:attribute name="xmi:id"><xsl:value-of select="concat(generate-id(),'_',$Idref)"/></xsl:attribute>
        <xsl:attribute name="xmi:idref"><xsl:value-of select="$Idref"/></xsl:attribute>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="packagedElement" mode="ImplementTemplateParameter">
    <xsl:param name="Idref"/>
    <xsl:param name="Parameter"/>
    <xsl:apply-templates select="templateParameter[@xmi:label=$Parameter]" mode="ImplementTemplateParameter">
      <xsl:with-param name="Idref" select="$Idref"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="templateParameter" mode="ImplementTemplateParameter">
    <xsl:param name="Idref"/>
    <xsl:copy>
      <xsl:copy-of select="@xmi:type"/>
      <xsl:copy-of select="@xmi:label"/>
      <xsl:attribute name="xmi:idref"><xsl:value-of select="@xmi:id"/></xsl:attribute>
      <xsl:element name="parameteredElement">
        <xsl:attribute name="xmi:type">uml:ParameterableElement</xsl:attribute>
        <xsl:attribute name="xmi:id"><xsl:value-of select="concat(generate-id(),'_',$Idref)"/></xsl:attribute>
        <xsl:attribute name="xmi:idref"><xsl:value-of select="$Idref"/></xsl:attribute>
      </xsl:element>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
</xsl:stylesheet>


