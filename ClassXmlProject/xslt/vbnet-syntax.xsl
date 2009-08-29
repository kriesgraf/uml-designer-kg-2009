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
  <xsl:variable name="Apos">'</xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="VbPredefinedValues">True;False;Nothing;</xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="Classes">
    <xsl:apply-templates select="//class" mode="UnknownClasses"/>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="UnknownTypes1">
    <xsl:apply-templates select="//class" mode="UnknownTypes"/>
    <xsl:apply-templates select="//inherited" mode="UnknownTypes"/>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="UnknownTypes">
    <xsl:for-each select="msxsl:node-set($UnknownTypes1)/*[generate-id()=generate-id(key('include',@name)[1])]">
      <xsl:sort select="@name"/>
        <xsl:variable name="Name">
          <xsl:apply-templates select="@name" mode="SimpleName2"/>
        </xsl:variable>
      <xsl:variable name="ClassId">
        <xsl:call-template name="ClassMember">
          <xsl:with-param name="Label" select="$Name"/>
        </xsl:call-template>
      </xsl:variable>
      <!--ICI ClassId="{$ClassId}" Label="{@name}" /-->
      <xsl:if test="$ClassId=''">
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
      </xsl:if>
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
  <xsl:template name="SearchSize">
    <xsl:param name="Label"/>
    <xsl:variable name="Idref">
      <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label]/@idref"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$Idref!=''">
        <xsl:attribute name="sizeref">
          <xsl:value-of select="$Idref"/>
        </xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
        <xsl:attribute name="size">
          <xsl:value-of select="$Label"/>
        </xsl:attribute>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="SearchValue">
    <xsl:param name="Label"/>
    <xsl:attribute name="Kind">
      <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label]/@kind"/>
    </xsl:attribute>
    <xsl:variable name="Idref">
      <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label]/@idref"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$Idref!=''">
        <xsl:attribute name="valref">
          <xsl:value-of select="$Idref"/>
        </xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
        <xsl:attribute name="value">
          <xsl:value-of select="$Label"/>
        </xsl:attribute>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="SearchSimpleType">
    <xsl:param name="Label"/>
    <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label]/@desc"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="attribute | param" mode="UnknownValues">
    <xsl:param name="ClassName"/>
    <xsl:variable name="Value" select="concat(@default,@value)"/>
    <xsl:choose>
      <xsl:when test="contains($VbPredefinedValues,concat($Value,';'))"/>
      <xsl:when test="contains($Value,'(')"/>
      <xsl:when test="starts-with($Value,'&quot;')"/>
      <xsl:when test="starts-with($Value,$Apos)"/>
      <xsl:when test="$Value!=''">
        <xsl:if test="string(number($Value))='NaN'">
          <xsl:variable name="Prefix">
            <xsl:apply-templates select="@default" mode="PrefixName"/>
            <xsl:apply-templates select="@value" mode="PrefixName"/>
          </xsl:variable>
          <xsl:variable name="Name">
            <xsl:choose>
              <xsl:when test="contains($Value,$ClassName)">
                <xsl:value-of select="$Value"/>
              </xsl:when>
              <xsl:when test="not(contains($Prefix,'.'))">
                <xsl:value-of select="concat($ClassName,'.',$Value)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$Value"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <element-type kind="Value" node="Value_1" name="{$Name}" container="0"/>
        </xsl:if>
      </xsl:when>
    </xsl:choose>
    <xsl:if test="@size!=''">
      <xsl:if test="string(number(@size))='NaN'">
        <xsl:variable name="Prefix">
          <xsl:apply-templates select="@size" mode="PrefixName"/>
        </xsl:variable>
        <xsl:variable name="Name">
          <xsl:choose>
            <xsl:when test="contains(@size,$ClassName)">
              <xsl:value-of select="@size"/>
            </xsl:when>
            <xsl:when test="not(contains($Prefix,'.'))">
              <xsl:value-of select="concat($ClassName,'.',@size)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="@size"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <element-type kind="Value" node="Size_1" name="{$Name}" container="0"/>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="UnknownClasses">
    <xsl:variable name="Prefix">
      <xsl:choose>
        <xsl:when test="ancestor::class">
          <xsl:apply-templates select="ancestor::class" mode="FullpathClassName"/>
        </xsl:when>
        <xsl:when test="parent::package">
          <xsl:apply-templates select="parent::package" mode="FullpathClassName"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
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
      <xsl:attribute name="prefix">
        <xsl:value-of select="$Prefix"/>
      </xsl:attribute>
    </xsl:copy>
    <xsl:apply-templates select="descendant::class" mode="UnknownClasses"/>
    <xsl:variable name="ClassName">
      <xsl:choose>
        <xsl:when test="$Prefix!=''">
          <xsl:value-of select="concat($Prefix,'.',@name)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@name"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:apply-templates select="descendant::attribute" mode="UnknownClasses">
      <xsl:with-param name="ClassName" select="$ClassName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="descendant::typedef" mode="UnknownClasses">
      <xsl:with-param name="ClassName" select="$ClassName"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="attribute" mode="UnknownClasses">
    <xsl:param name="ClassName"/>
    <xsl:if test="@default and @default!='' and contains(@other,'Const')">
      <class id="{generate-id()}" kind="Constant" prefix="{$ClassName}">
        <xsl:copy-of select="@name"/>
      </class>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef" mode="UnknownClasses">
    <xsl:param name="ClassName"/>
    <class id="{generate-id()}" kind="{@type}" prefix="{$ClassName}">
      <xsl:copy-of select="@name"/>
    </class>
    <xsl:apply-templates select="descendant::enumvalue" mode="UnknownClasses">
      <xsl:with-param name="ClassName" select="concat($ClassName,'.',@name)"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="enumvalue" mode="UnknownClasses">
    <xsl:param name="ClassName"/>
    <class id="{generate-id()}" kind="Value" prefix="{$ClassName}">
      <xsl:copy-of select="@name"/>
    </class>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="UnknownTypes">
    <xsl:apply-templates select="descendant::method | descendant::property | descendant::attribute | descendant::typedef" mode="UnknownTypes">
      <xsl:with-param name="ClassName" select="@name"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="descendant::attribute" mode="UnknownValues">
      <xsl:with-param name="ClassName" select="@name"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="descendant::param" mode="UnknownValues">
      <xsl:with-param name="ClassName" select="@name"/>
    </xsl:apply-templates>
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
    <element-type kind="Class" node="{name()}_3" container="0">
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
    <element-type kind="Class" node="{name()}_4" name="{@name}">
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
  <xsl:template match="type" mode="UnknownTypes">
    <xsl:variable name="Name">
      <xsl:apply-templates select="@desc" mode="SimpleName"/>
    </xsl:variable>
    <xsl:if test="not(contains($Name,'('))">
      <element-type kind="Class" node="{name()}_5" container="0">
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
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@value | @size" mode="UnknownTypes">
    <xsl:param name="ParamName"/>
    <xsl:variable name="Name">
      <xsl:apply-templates select="." mode="SimpleName"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="string(number($Name))!='NaN'"/>
      <xsl:when test="contains($VbPredefinedValues,concat($Name,';'))"/>
      <xsl:when test="contains($Name,'(')"/>
      <xsl:when test="starts-with($Name,'&quot;')"/>
      <xsl:when test="starts-with($Name,$Apos)"/>
      <xsl:when test="contains($Name,',')">
        <element-type kind="Class" node="{$ParamName}_6" container="0">
          <xsl:attribute name="name">
            <xsl:value-of select="substring-after($Name,',')"/>
          </xsl:attribute>
          <xsl:attribute name="prefix">
            <xsl:value-of select="substring-before($Name,',')"/>
          </xsl:attribute>
        </element-type>
      </xsl:when>
      <xsl:otherwise>
        <element-type kind="Class" node="{$ParamName}_6" container="0">
          <xsl:attribute name="name">
            <xsl:value-of select="$Name"/>
          </xsl:attribute>
        </element-type>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="method" mode="UnknownTypes">
    <xsl:if test="@return">
      <element-type kind="Class" node="{name()}_7" name="{@return}" container="0"/>
    </xsl:if>
    <xsl:variable name="ListParams">
      <xsl:apply-templates select="param"/>
    </xsl:variable>
    <xsl:for-each select="msxsl:node-set($ListParams)/param">
      <xsl:apply-templates select="type" mode="UnknownTypes"/>
      <xsl:apply-templates select="variable/@value" mode="UnknownTypes">
        <xsl:with-param name="ParamName">Value</xsl:with-param>
      </xsl:apply-templates>
      <xsl:apply-templates select="variable/@size" mode="UnknownTypes">
        <xsl:with-param name="ParamName">Size</xsl:with-param>
      </xsl:apply-templates>
    </xsl:for-each>
  </xsl:template>
  <!-- ============================================================================== -->
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
  <xsl:template match="variable" mode="Value">
    <xsl:copy>
      <xsl:copy-of select="@*[name()!='value' and name()!='size']"/>
      <xsl:if test="@value">
        <xsl:variable name="Value">
          <xsl:apply-templates select="@value" mode="SimpleName2"/>
        </xsl:variable>
        <xsl:call-template name="SearchValue">
          <xsl:with-param name="Label" select="$Value"/>
        </xsl:call-template>
      </xsl:if>
      <xsl:if test="@size">
        <xsl:variable name="Size">
          <xsl:apply-templates select="@size" mode="SimpleName2"/>
        </xsl:variable>
        <xsl:call-template name="SearchSize">
          <xsl:with-param name="Label" select="$Size"/>
        </xsl:call-template>
      </xsl:if>
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
  <xsl:template match="@type | @desc | @name | @default | @value | @size" mode="PrefixName">
    <xsl:variable name="Name">
      <xsl:apply-templates select="." mode="SimpleName"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="starts-with(.,'&quot;')"/>
      <xsl:when test="contains(.,'(')"/>
      <xsl:when test="contains($Name,',')">
        <xsl:value-of select="substring-before($Name,',')"/>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@type | @desc | @name | @default | @value | @size" mode="SimpleName2">
    <xsl:variable name="Name">
      <xsl:apply-templates select="." mode="SimpleName"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="starts-with(.,'&quot;')">
        <xsl:value-of select="."/>
      </xsl:when>
      <xsl:when test="starts-with(.,$Apos)">
        <xsl:value-of select="."/>
      </xsl:when>
      <xsl:when test="contains(.,'(')">
        <xsl:value-of select="."/>
      </xsl:when>
      <xsl:when test="contains($Name,',')">
        <xsl:value-of select="substring-after($Name,',')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$Name"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@type | @desc | @name | @default | @value | @size" mode="SimpleName">
    <xsl:choose>
      <xsl:when test="starts-with(.,'&quot;')">
        <xsl:value-of select="."/>
      </xsl:when>
      <xsl:when test="starts-with(.,$Apos)">
        <xsl:value-of select="."/>
      </xsl:when>
      <xsl:when test="contains(.,'(')">
        <xsl:value-of select="."/>
      </xsl:when>
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































































