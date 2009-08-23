<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
<!-- ======================================================================= -->
  <xsl:key match="element-type" name="include" use="@name"/>
  <xsl:key match="import" name="import" use="@name"/>
  <xsl:key match="element-type" name="package" use="@prefix"/>
  <!-- ======================================================================= -->
  <xsl:variable name="Language">
    <xsl:if test="$LanguageFolder=''">
      <xsl:message terminate="yes">Parameter $LanguageFolder not yet filled!</xsl:message>
    </xsl:if>
    <xsl:value-of select="$LanguageFolder"/>
    <xsl:text>\LanguageVbasic.xml</xsl:text>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="Classes">
    <xsl:apply-templates select="//typedef" mode="UnknownClasses"/>
    <xsl:apply-templates select="//class" mode="UnknownClasses"/>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="UnknownTypes1">
    <xsl:apply-templates select="//method[@return!=''] | //property | //attribute | //typedef | //inherited" mode="UnknownTypes"/>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="UnknownTypes">
    <xsl:for-each select="msxsl:node-set($UnknownTypes1)/*[generate-id()=generate-id(key('include',@name)[1])]">
      <xsl:sort select="@name"/>
      <xsl:variable name="Name">
        <xsl:apply-templates select="@name" mode="SimpleName2"/>
      </xsl:variable>
      <xsl:variable name="Prefix">
        <xsl:apply-templates select="@name" mode="PrefixName"/>
      </xsl:variable>
      <xsl:variable name="Description">
        <xsl:call-template name="SimpleTypes">
          <xsl:with-param name="Label" select="$Name"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Idref">
        <xsl:call-template name="ClassMember">
          <xsl:with-param name="Label" select="$Name"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:copy>
        <xsl:copy-of select="@*"/>
        <xsl:if test="$Prefix!=''">
          <xsl:attribute name="name">
            <xsl:value-of select="$Name"/>
          </xsl:attribute>
          <xsl:attribute name="prefix">
            <xsl:value-of select="$Prefix"/>
          </xsl:attribute>
        </xsl:if>
        <!--xsl:attribute name="RESULT">
          <xsl:value-of select="concat($Prefix,':',$Name,'=[',$Description,'|',$Idref,']')"/>
        </xsl:attribute-->
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
          <xsl:otherwise>
            <xsl:attribute name="import">yes</xsl:attribute>
            <xsl:attribute name="idref">
              <xsl:value-of select="generate-id()"/>
            </xsl:attribute>
          </xsl:otherwise>
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
    <!--xsl:value-of select="concat('[',$Label,']=')"/-->
    <xsl:value-of select="msxsl:node-set($Classes)/*[@name=$Label]/@id"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="SearchMember">
    <xsl:param name="Label"/>
    <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label]/@idref"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="SearchSimpleType">
    <xsl:param name="Label"/>
    <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label]/@desc"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="UnknownClasses">
    <xsl:copy>
      <xsl:attribute name="id">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="ancestor::class">
          <xsl:attribute name="kind">Container</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@kind"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:copy-of select="@name"/>
      <xsl:choose>
        <xsl:when test="ancestor::class">
          <xsl:attribute name="prefix">
            <xsl:apply-templates select="ancestor::class" mode="FullpathClassName"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="parent::package">
          <xsl:attribute name="prefix">
            <xsl:apply-templates select="parent::package" mode="FullpathClassName"/>
          </xsl:attribute>
        </xsl:when>
      </xsl:choose>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef" mode="UnknownClasses">
    <class id="{generate-id()}" kind="{@type}">
      <xsl:copy-of select="@name"/>
      <xsl:attribute name="prefix">
        <xsl:apply-templates select="." mode="FullpathClassName"/>
      </xsl:attribute>
    </class>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef" mode="UnknownTypes">
    <xsl:apply-templates select="element" mode="UnknownTypes"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="property | attribute | element" mode="UnknownTypes">
    <xsl:variable name="Name">
      <xsl:apply-templates select="@type" mode="SimpleName"/>
    </xsl:variable>
    <element-type node="{name()}" container="0">
      <xsl:choose>
        <xsl:when test="contains($Name,',')">
          <xsl:attribute name="name">
            <xsl:value-of select="substring-after($Name,',')"/>
          </xsl:attribute>
          <xsl:attribute name="prefix">
            <xsl:value-of select="substring-before($Name,',')"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="name">
            <xsl:value-of select="$Name"/>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
    </element-type>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited" mode="UnknownTypes">
    <element-type node="{name()}" name="{@name}">
      <xsl:attribute name="container">
        <xsl:choose>
          <xsl:when test="not(@template)">0</xsl:when>
          <xsl:when test="contains(@template,',')">2</xsl:when>
          <xsl:otherwise>1</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
    </element-type>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="method" mode="UnknownTypes">
    <element-type node="{name()}" name="{@return}" container="0"/>
    <xsl:variable name="ListParams">
      <xsl:call-template name="PARAMS">
        <xsl:with-param name="Params" select="@params"/>
        <xsl:with-param name="Types" select="@types"/>
        <xsl:with-param name="Values" select="@values"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:for-each select="msxsl:node-set($ListParams)/param">
      <xsl:variable name="Name">
        <xsl:apply-templates select="type/@desc" mode="SimpleName"/>
      </xsl:variable>
      <element-type node="{name()}" container="0">
        <xsl:choose>
          <xsl:when test="contains($Name,',')">
            <xsl:attribute name="name">
              <xsl:value-of select="substring-after($Name,',')"/>
            </xsl:attribute>
            <xsl:attribute name="prefix">
              <xsl:value-of select="substring-before($Name,',')"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="name">
              <xsl:value-of select="$Name"/>
            </xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
      </element-type>
    </xsl:for-each>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template name="AllParams">
    <xsl:param name="Comments"/>
    <xsl:variable name="Params">
      <xsl:call-template name="PARAMS">
        <xsl:with-param name="Params" select="@params"/>
        <xsl:with-param name="Types" select="@types"/>
        <xsl:with-param name="Values" select="@values"/>
        <xsl:with-param name="Comments" select="$Comments"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:for-each select="msxsl:node-set($Params)/param">
      <xsl:copy>
        <xsl:copy-of select="@*"/>
        <xsl:apply-templates select="type" mode="Type"/>
        <xsl:copy-of select="variable"/>
        <xsl:copy-of select="comment"/>
      </xsl:copy>
    </xsl:for-each>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template name="PARAMS">
    <xsl:param name="Params"/>
    <xsl:param name="Types"/>
    <xsl:param name="Values"/>
    <xsl:param name="Comments"/>
    <xsl:choose>
      <xsl:when test="contains($Params,',')">
        <xsl:call-template name="ImplementsPARAMS">
          <xsl:with-param name="Param" select="substring-before($Params,',')"/>
          <xsl:with-param name="Type" select="substring-before($Types,',')"/>
          <xsl:with-param name="Value" select="substring-before($Values,',')"/>
          <xsl:with-param name="Comments" select="$Comments"/>
        </xsl:call-template>
        <xsl:call-template name="PARAMS">
          <xsl:with-param name="Params" select="substring-after($Params,',')"/>
          <xsl:with-param name="Types" select="substring-after($Types,',')"/>
          <xsl:with-param name="Values" select="substring-after($Values,',')"/>
          <xsl:with-param name="Comments" select="$Comments"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="$Params=''"/>
      <xsl:otherwise>
        <xsl:call-template name="ImplementsPARAMS">
          <xsl:with-param name="Param" select="$Params"/>
          <xsl:with-param name="Type" select="$Types"/>
          <xsl:with-param name="Value" select="$Values"/>
          <xsl:with-param name="Comments" select="$Comments"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@type | @return" mode="Type">
    <xsl:variable name="Description">
      <xsl:call-template name="SearchSimpleType">
        <xsl:with-param name="Label" select="."/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Idref">
      <xsl:call-template name="SearchMember">
        <xsl:with-param name="Label" select="."/>
      </xsl:call-template>
    </xsl:variable>
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
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type" mode="Type">
    <xsl:variable name="Description">
      <xsl:call-template name="SearchSimpleType">
        <xsl:with-param name="Label" select="@desc"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Idref">
      <xsl:call-template name="SearchMember">
        <xsl:with-param name="Label" select="@desc"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:copy>
      <xsl:copy-of select="@*[name()!='desc']"/>
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
      <xsl:copy-of select="*"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited" mode="Typedef">
    <xsl:variable name="Description">
      <xsl:call-template name="SearchSimpleType">
        <xsl:with-param name="Label" select="@name"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Idref">
      <xsl:call-template name="SearchMember">
        <xsl:with-param name="Label" select="@name"/>
      </xsl:call-template>
    </xsl:variable>
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
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited" mode="List">
    <xsl:variable name="Description">
      <xsl:call-template name="SearchSimpleType">
        <xsl:with-param name="Label" select="@name"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Idref">
      <xsl:call-template name="SearchMember">
        <xsl:with-param name="Label" select="@name"/>
      </xsl:call-template>
    </xsl:variable>
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
    <xsl:choose>
      <xsl:when test="contains(@template,',')">
        <xsl:variable name="IndexDescription">
          <xsl:call-template name="SearchSimpleType">
            <xsl:with-param name="Label" select="substring-before(@template,',')"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="IndexIdref">
          <xsl:call-template name="SearchMember">
            <xsl:with-param name="Label" select="substring-before(@template,',')"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$IndexDescription!=''">
            <xsl:attribute name="index-desc">
              <xsl:value-of select="$IndexDescription"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="$IndexIdref!=''">
            <xsl:attribute name="index-idref">
              <xsl:value-of select="$IndexIdref"/>
            </xsl:attribute>
          </xsl:when>
        </xsl:choose>
        <xsl:attribute name="type">indexed</xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
        <xsl:attribute name="type">simple</xsl:attribute>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="property | attribute" mode="List">
    <xsl:variable name="Description">
      <xsl:call-template name="SearchSimpleType">
        <xsl:with-param name="Label" select="@type"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Idref">
      <xsl:call-template name="SearchMember">
        <xsl:with-param name="Label" select="@type"/>
      </xsl:call-template>
    </xsl:variable>
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
    <xsl:choose>
      <xsl:when test="contains(@template,',')">
        <xsl:variable name="IndexDescription">
          <xsl:call-template name="SearchSimpleType">
            <xsl:with-param name="Label" select="substring-before(@template,',')"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="IndexIdref">
          <xsl:call-template name="SearchMember">
            <xsl:with-param name="Label" select="substring-before(@template,',')"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$IndexDescription!=''">
            <xsl:attribute name="index-desc">
              <xsl:value-of select="$IndexDescription"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:when test="$IndexIdref!=''">
            <xsl:attribute name="index-idref">
              <xsl:value-of select="$IndexIdref"/>
            </xsl:attribute>
          </xsl:when>
        </xsl:choose>
        <xsl:attribute name="type">indexed</xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
        <xsl:attribute name="type">simple</xsl:attribute>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited | property | attribute" mode="Type">
    <xsl:variable name="Name">
      <xsl:choose>
        <xsl:when test="contains(@template,',')">
          <xsl:value-of select="substring-after(@template,',')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@template"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Description">
      <xsl:call-template name="SearchSimpleType">
        <xsl:with-param name="Label" select="$Name"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Idref">
      <xsl:call-template name="SearchMember">
        <xsl:with-param name="Label" select="$Name"/>
      </xsl:call-template>
    </xsl:variable>
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
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="UnknownImports">
    <xsl:for-each select="msxsl:node-set($UnknownTypes)/*[@import='yes' and (not(@prefix) or @prefix='')]">
      <xsl:apply-templates select="." mode="Imports"/>
    </xsl:for-each>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Imports">
    <xsl:variable name="Imports">
      <xsl:apply-templates select="//imports"/>
    </xsl:variable>
    <xsl:for-each select="msxsl:node-set($Imports)/*[generate-id()=generate-id(key('import',@name)[1])]">
      <xsl:sort select="@name"/>
      <xsl:copy-of select="."/>
    </xsl:for-each>
    <xsl:for-each select="msxsl:node-set($UnknownTypes)/*[@import='yes' and @prefix!=''][generate-id()=generate-id(key('package',@prefix)[1])]">
      <xsl:sort select="@prefix"/>
      <xsl:variable name="Prefix" select="@prefix"/>
      <xsl:element name="import">
        <xsl:attribute name="name">
          <xsl:value-of select="@prefix"/>
        </xsl:attribute>
        <xsl:attribute name="visibility">package</xsl:attribute>
        <xsl:element name="export">
          <xsl:attribute name="name">
            <xsl:value-of select="@prefix"/>
          </xsl:attribute>
          <xsl:for-each select="msxsl:node-set($UnknownTypes)/*[@prefix=$Prefix]">
            <xsl:apply-templates select="." mode="Imports">
              <xsl:with-param name="NoPrefix">No</xsl:with-param>
            </xsl:apply-templates>
          </xsl:for-each>
        </xsl:element>
      </xsl:element>
    </xsl:for-each>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="ParamComment">
    <xsl:param name="Comments"/>
    <xsl:param name="ParamName"/>
    <xsl:if test="not(msxsl:node-set($Comments)/*)">
      <xsl:element name="comment">Insert here a comment</xsl:element>
    </xsl:if>
    <xsl:apply-templates select="msxsl:node-set($Comments)/*" mode="Param">
      <xsl:with-param name="ParamName" select="$ParamName"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="Signature">
    <xsl:copy>
      <xsl:copy-of select="@name"/>
      <xsl:copy-of select="@kind"/>
      <xsl:copy-of select="@other"/>
      <xsl:apply-templates select="descendant::method[contains(@other,'MustOverride') or contains(@other,'Overridable')]" mode="Signature">
        <xsl:with-param name="ClassID" select="generate-id()"/>
      </xsl:apply-templates>
      <xsl:if test="@kind='Interface'">
        <xsl:apply-templates select="descendant::method" mode="Signature">
          <xsl:with-param name="ClassID" select="generate-id()"/>
        </xsl:apply-templates>
      </xsl:if>
      <xsl:apply-templates select="descendant::inherited" mode="Signature"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited" mode="Signature">
    <xsl:variable name="ClassName" select="@name"/>
    <xsl:apply-templates select="//class[@name=$ClassName]" mode="Signature"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="method" mode="Signature">
    <xsl:param name="ClassID"/>
    <signature name="{concat(@name,'(',@types,')')}">
      <xsl:copy-of select="@other"/>
      <xsl:attribute name="id">
        <xsl:value-of select="$ClassID"/>
      </xsl:attribute>
    </signature>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="ShouldInherit">
    <xsl:apply-templates select="descendant::inherited" mode="Signature"/>
    <!--xsl:variable name="ShouldInherit">
    </xsl:variable>
    <xsl:for-each select="msxsl:node-set($ShouldInherit)//signature">
      <xsl:copy-of select="."/>
    </xsl:for-each-->
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="method" mode="Overrides">
    <xsl:param name="ShouldInherit"/>
    <xsl:if test="@implements or contains(@other,'Overrides')">
      <xsl:variable name="Signature">
        <xsl:variable name="Signature2">
          <xsl:apply-templates select="." mode="Signature"/>
        </xsl:variable>
        <xsl:value-of select="msxsl:node-set($Signature2)//signature/@name"/>
      </xsl:variable>
      <xsl:value-of select="msxsl:node-set($ShouldInherit)//signature[@name=$Signature]/@id"/>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@type | @desc | @name" mode="PrefixName">
    <xsl:variable name="Name">
      <xsl:apply-templates select="." mode="SimpleName"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Name,',')">
        <xsl:value-of select="substring-before($Name,',')"/>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@type | @desc | @name" mode="SimpleName2">
    <xsl:variable name="Name">
      <xsl:apply-templates select="." mode="SimpleName"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Name,',')">
        <xsl:value-of select="substring-after($Name,',')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$Name"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@type | @desc | @name" mode="SimpleName">
    <xsl:choose>
      <xsl:when test="contains(.,'.')">
        <xsl:value-of select="substring-before(.,'.')"/>
        <xsl:if test="contains(substring-after(.,'.'),'.')">
          <xsl:text>.</xsl:text>
        </xsl:if>
        <xsl:call-template name="SimpleName">
          <xsl:with-param name="Name" select="substring-after(.,'.')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="."/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="SimpleName">
    <xsl:param name="Name"/>
    <xsl:choose>
      <xsl:when test="contains($Name,'.')">
        <xsl:value-of select="substring-before($Name,'.')"/>
        <xsl:if test="contains(substring-after($Name,'.'),'.')">
          <xsl:text>.</xsl:text>
        </xsl:if>
        <xsl:call-template name="SimpleName">
          <xsl:with-param name="Name" select="substring-after($Name,'.')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat(',',$Name)"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="*" mode="FullpathClassName">
    <xsl:if test="parent::package">
      <xsl:apply-templates select="parent::package" mode="FullpathClassName"/>
      <xsl:text>.</xsl:text>
    </xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="text()"/>
  <!-- ======================================================================= -->
</xsl:stylesheet>


































