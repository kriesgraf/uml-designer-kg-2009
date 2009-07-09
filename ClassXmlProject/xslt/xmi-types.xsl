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
    <xsl:if test="$LanguageFolder=''">
      <xsl:message terminate="yes">XSLT parameter 'LanguageFolder' is empty!</xsl:message>
    </xsl:if>
    <xsl:value-of select="$LanguageFolder"/>
    <xsl:choose>
      <xsl:when test="$LanguageValue='0'">\LanguageCplusPlus.xml</xsl:when>
      <xsl:when test="$LanguageValue='1'">\LanguageVbasic.xml</xsl:when>
      <xsl:otherwise>\LanguageJava.xml</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <!-- ======================================================================= -->
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
            <packagedElement xmi:type="uml:DataType" name="{.}"/>
            <!-- xmi:label="{.}"/-->
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>
    </xsl:for-each>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="PredefinedTypes">
    <xsl:apply-templates select="msxsl:node-set($PredefinedTypes2)/*" mode="Renumber"/>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:template match="packagedElement" mode="Renumber">
    <xsl:copy>
      <xsl:copy-of select="@*"/>
      <xsl:attribute name="xmi:id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
      <xsl:apply-templates select="*" mode="Renumber">
        <xsl:with-param name="ClassID" select="generate-id()"/>
      </xsl:apply-templates>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedTemplateSignature" mode="Renumber">
    <xsl:param name="ClassID"/>
    <xsl:copy>
      <xsl:copy-of select="@*"/>
      <xsl:attribute name="template">
        <xsl:value-of select="$ClassID"/>
      </xsl:attribute>
      <xsl:copy-of select="*"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="nestedClassifier" mode="Renumber">
    <xsl:copy-of select="."/>
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
  <xsl:template match="list" mode="ImplementTemplate">
    <xsl:param name="TypeID"/>
    <xsl:param name="TypedefID"/>
    <xsl:variable name="ListID">
      <xsl:choose>
        <xsl:when test="@desc">
          <xsl:apply-templates select="@desc" mode="Code"/>
        </xsl:when>
        <xsl:when test="@idref">
          <xsl:value-of select="@idref"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="NodeList">
      <xsl:choose>
        <xsl:when test="id($ListID)[self::class]">
          <xsl:apply-templates select="id($ListID)" mode="TemplateSignature"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="msxsl:node-set($PredefinedTypes)//*[@xmi:id=$ListID]/ownedTemplateSignature"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="SignatureID" select="msxsl:node-set($NodeList)//ownedTemplateSignature/@xmi:id"/>
    <xsl:variable name="ValueID" select="msxsl:node-set($NodeList)//ownedParameter[1]/@xmi:id"/>
    <xsl:element name="templateBinding">
      <xsl:attribute name="xmi:type">uml:TemplateBinding</xsl:attribute>
      <xsl:attribute name="xmi:id">
        <xsl:value-of select="generate-id()"/>
        <xsl:text>_tmpl</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="signature">
        <xsl:value-of select="$SignatureID"/>
      </xsl:attribute>
      <xsl:attribute name="boundElement">
        <xsl:value-of select="$TypedefID"/>
      </xsl:attribute>
      <xsl:call-template name="TemplateBinding">
        <xsl:with-param name="Index">1</xsl:with-param>
        <xsl:with-param name="TypeID" select="$TypeID"/>
        <xsl:with-param name="ValueID" select="$ValueID"/>
      </xsl:call-template>
      <xsl:if test="@type='indexed'">
        <xsl:variable name="IndexID">
          <xsl:choose>
            <xsl:when test="@index-desc">
              <xsl:apply-templates select="@index-desc" mode="Code"/>
            </xsl:when>
            <xsl:when test="@index-idref">
              <xsl:value-of select="@index-idref"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="KeyID" select="msxsl:node-set($NodeList)//ownedParameter[2]/@xmi:id"/>
        <xsl:call-template name="TemplateBinding">
          <xsl:with-param name="Index">2</xsl:with-param>
          <xsl:with-param name="TypeID" select="$IndexID"/>
          <xsl:with-param name="ValueID" select="$KeyID"/>
        </xsl:call-template>
      </xsl:if>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="packagedElement" mode="ImplementTemplateParameter">
    <xsl:param name="Idref"/>
    <xsl:param name="Parameter"/>
    <!--xsl:apply-templates select="templateParameter[@xmi:label=$Parameter]" mode="ImplementTemplateParameter"-->
    <xsl:apply-templates select="templateParameter[@name=$Parameter]" mode="ImplementTemplateParameter">
      <xsl:with-param name="Idref" select="$Idref"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="templateParameter" mode="ImplementTemplateParameter">
    <xsl:param name="Idref"/>
    <xsl:copy>
      <xsl:copy-of select="@xmi:type"/>
      <xsl:copy-of select="@name"/>
      <!--xsl:copy-of select="@xmi:label"/-->
      <xsl:attribute name="xmi:idref">
        <xsl:value-of select="@xmi:id"/>
      </xsl:attribute>
      <xsl:element name="parameteredElement">
        <xsl:attribute name="xmi:type">uml:ParameterableElement</xsl:attribute>
        <xsl:attribute name="xmi:id">
          <xsl:value-of select="concat(generate-id(),'_',$Idref)"/>
        </xsl:attribute>
        <xsl:attribute name="xmi:idref">
          <xsl:value-of select="$Idref"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
</xsl:stylesheet>





















