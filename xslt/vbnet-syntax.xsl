<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
<!-- ======================================================================= -->
  <xsl:key match="element-type" name="include" use="@name"/>
  <xsl:key match="import" name="import" use="@name"/>
  <xsl:key match="element-type" name="package" use="@prefix"/>
  <xsl:key match="element-package" name="import" use="@name"/>
  <xsl:key match="@prefix" name="prefix" use="."/>
  <!-- ======================================================================= -->
  <xsl:variable name="LanguageVbasic">
    <xsl:if test="$LanguageFolder=''">
      <xsl:message terminate="yes">Parameter $LanguageFolder not yet filled!</xsl:message>
    </xsl:if>
    <xsl:value-of select="$LanguageFolder"/>
    <xsl:text>\LanguageVbasic.xml</xsl:text>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:template name="SimpleTypes">
    <xsl:param name="Label"/>
    <xsl:choose>
      <xsl:when test="contains($VbPredefinedValues,concat($Label,';'))">
        <xsl:value-of select="$Label"/>
      </xsl:when>
      <xsl:when test="contains($Label,'(')">
        <xsl:value-of select="$Label"/>
      </xsl:when>
      <xsl:when test="starts-with($Label,'&quot;')">
        <xsl:value-of select="$Label"/>
      </xsl:when>
      <xsl:when test="starts-with($Label,$Apos)">
        <xsl:value-of select="$Label"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="document($LanguageVbasic)//*[@implementation=$Label]/@name"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:variable name="Apos">'</xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="VbPredefinedValues">True;False;Nothing;</xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="Classes">
    <xsl:apply-templates select="//class" mode="KnownClasses"/>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="UnknownTypes1">
    <!-- TODO: insert here importation of Framwork DLL -->
    <element-type kind="Class" node="predefined" name="Object" prefix="System" import="yes" container="0"/>
    <element-type kind="Class" node="predefined" name="Exception" prefix="System" import="yes" container="0"/>
    <xsl:apply-templates select="//class" mode="UnknownTypes"/>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="UnknownPackage1">
    <xsl:variable name="Imports">
      <xsl:for-each select="msxsl:node-set($UnknownTypes1)/*/@prefix[.!=''][generate-id()=generate-id(key('prefix',.)[1])]">
        <element-package name="{.}"/>
      </xsl:for-each>
      <xsl:for-each select="//imports">
        <element-package name="{@name}"/>
        <xsl:for-each select="node-import">
          <element-package name="{.}"/>
        </xsl:for-each>
      </xsl:for-each>
    </xsl:variable>
    <xsl:for-each select="msxsl:node-set($Imports)/*[generate-id()=generate-id(key('import',@name)[1])]">
      <xsl:sort select="@name"/>
      <xsl:copy-of select="."/>
    </xsl:for-each>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:variable name="UnknownTypes">
    <xsl:for-each select="msxsl:node-set($UnknownTypes1)/*[@kind='Class' and @prefix=''][generate-id()=generate-id(key('include',@name)[1])]">
      <xsl:sort select="@name"/>
      <xsl:apply-templates select="." mode="Copy"/>
    </xsl:for-each>
    <xsl:for-each select="msxsl:node-set($UnknownTypes1)/*[@kind='Constant' and @prefix=''][generate-id()=generate-id(key('include',@name)[1])]">
      <xsl:sort select="@name"/>
      <xsl:apply-templates select="." mode="Copy"/>
    </xsl:for-each>
    <xsl:for-each select="msxsl:node-set($UnknownTypes1)/*[@kind='Value' and @prefix=''][generate-id()=generate-id(key('include',@name)[1])]">
      <xsl:sort select="@name"/>
      <xsl:apply-templates select="." mode="Copy"/>
    </xsl:for-each>
    <xsl:for-each select="msxsl:node-set($UnknownPackage1)/*">
      <xsl:variable name="Name" select="@name"/>
      <xsl:for-each select="msxsl:node-set($UnknownTypes1)/*[@kind='Class' and @prefix=$Name][generate-id()=generate-id(key('include',@name)[1])]">
        <xsl:sort select="@name"/>
        <xsl:apply-templates select="." mode="Copy"/>
      </xsl:for-each>
      <xsl:for-each select="msxsl:node-set($UnknownTypes1)/*[@kind='Constant' and @prefix=$Name][generate-id()=generate-id(key('include',@name)[1])]">
        <xsl:sort select="@name"/>
        <xsl:apply-templates select="." mode="Copy"/>
      </xsl:for-each>
      <xsl:for-each select="msxsl:node-set($UnknownTypes1)/*[@kind='Value' and @prefix=$Name][generate-id()=generate-id(key('include',@name)[1])]">
        <xsl:sort select="@name"/>
        <xsl:apply-templates select="." mode="Copy"/>
      </xsl:for-each>
    </xsl:for-each>
  </xsl:variable>
  <!-- ======================================================================= -->
  <xsl:template name="Imports">
    <xsl:for-each select="msxsl:node-set($UnknownPackage1)/*">
      <xsl:sort select="@name"/>
      <xsl:variable name="Name" select="@name"/>
      <xsl:element name="import">
        <xsl:copy-of select="@name"/>
          <xsl:attribute name="param">
          <xsl:value-of select="$Name"/>
        </xsl:attribute>
        <xsl:attribute name="visibility">package</xsl:attribute>
        <xsl:element name="export">
          <xsl:copy-of select="@name"/>
          <xsl:for-each select="msxsl:node-set($UnknownTypes)/*[@kind='Class' and @prefix=$Name]">
            <xsl:apply-templates select="." mode="Imports">
              <xsl:with-param name="NoPrefix">No</xsl:with-param>
            </xsl:apply-templates>
          </xsl:for-each>
        </xsl:element>
      </xsl:element>
    </xsl:for-each>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="element-type" mode="Copy">
    <xsl:copy>
      <xsl:copy-of select="@*"/>
      <xsl:attribute name="idref">
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="classKnownClasses" match="class" mode="KnownClasses">
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
      <xsl:attribute name="kind">Class</xsl:attribute>
      <xsl:attribute name="container">
        <xsl:choose>
          <xsl:when test="not(@template)">0</xsl:when>
          <xsl:when test="contains(@template,',')">2</xsl:when>
          <xsl:otherwise>1</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:copy-of select="@name"/>
      <xsl:attribute name="prefix">
        <xsl:value-of select="$Prefix"/>
      </xsl:attribute>
    </xsl:copy>
    <xsl:apply-templates select="descendant::class" mode="KnownClasses"/>
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
    <xsl:apply-templates select="descendant::attribute" mode="KnownClasses">
      <xsl:with-param name="ClassName" select="$ClassName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="descendant::typedef" mode="KnownClasses">
      <xsl:with-param name="ClassName" select="$ClassName"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="attributeKnownClasses" match="attribute" mode="KnownClasses">
    <xsl:param name="ClassName"/>
    <xsl:if test="@default and @default!='' and contains(@other,'Const')">
      <class id="{generate-id()}" kind="Constant" prefix="{$ClassName}">
        <xsl:copy-of select="@name"/>
      </class>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="typedefKnownClasses" match="typedef" mode="KnownClasses">
    <xsl:param name="ClassName"/>
    <class id="{generate-id()}" kind="{@type}" prefix="{$ClassName}">
      <xsl:copy-of select="@name"/>
    </class>
    <xsl:apply-templates select="descendant::enumvalue" mode="KnownClasses">
      <xsl:with-param name="ClassName" select="concat($ClassName,'.',@name)"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="enumvalueKnownClasses" match="enumvalue" mode="KnownClasses">
    <xsl:param name="ClassName"/>
    <class id="{generate-id()}" kind="Value" prefix="{$ClassName}">
      <xsl:copy-of select="@name"/>
    </class>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="classUnknownTypes" match="class" mode="UnknownTypes">
    <xsl:apply-templates select="inherited" mode="UnknownTypes"/>
    <xsl:apply-templates select="descendant::class" mode="UnknownTypes"/>
    <xsl:apply-templates select="descendant::method | descendant::property" mode="UnknownTypes"/>
    <xsl:apply-templates select="descendant::typedef" mode="UnknownTypes"/>
    <xsl:apply-templates select="descendant::attribute" mode="UnknownTypes"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="inheritedUnknownTypes" match="inherited" mode="UnknownTypes">
    <xsl:variable name="Elements">
      <xsl:call-template name="NotifyUnknownTypes">
        <xsl:with-param name="ParamName" select="name()"/>
        <xsl:with-param name="TypeName" select="@name"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Container">
      <xsl:choose>
        <xsl:when test="contains(@template,',')">2</xsl:when>
        <xsl:when test="@template">1</xsl:when>
        <xsl:when test="contains(concat(@name,';'),'List;')">3</xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:element name="element-type">
      <xsl:copy-of select="msxsl:node-set($Elements)/*/@*"/>
      <xsl:attribute name="container">
        <xsl:value-of select="$Container"/>
      </xsl:attribute>
    </xsl:element>
    <xsl:apply-templates select="@template" mode="UnknownTypes">
      <xsl:with-param name="ParamName" select="name()"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="templateUnknownTypes" match="@template" mode="UnknownTypes">
    <xsl:param name="ParamName"/>
    <xsl:choose>
      <xsl:when test="contains(.,',')">
        <xsl:call-template name="NotifyUnknownTypes">
          <xsl:with-param name="ParamName" select="$ParamName"/>
          <xsl:with-param name="TypeName" select="substring-before(.,',')"/>
        </xsl:call-template>
        <xsl:call-template name="NotifyUnknownTypes">
          <xsl:with-param name="ParamName" select="$ParamName"/>
          <xsl:with-param name="TypeName" select="substring-after(.,',')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="NotifyUnknownTypes">
          <xsl:with-param name="ParamName" select="$ParamName"/>
          <xsl:with-param name="TypeName" select="."/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="typedefUnknownTypes" match="typedef" mode="UnknownTypes">
    <xsl:apply-templates select="element" mode="UnknownTypes"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="paepUnknownTypes" match="property | attribute | element | param" mode="UnknownTypes">
    <xsl:variable name="NodeType">
    <xsl:apply-templates select="@type" mode="UnknownTypes">
      <xsl:with-param name="ParamName" select="name()"/>
    </xsl:apply-templates>
    </xsl:variable>
    <xsl:copy-of select="$NodeType"/>
    <xsl:if test="msxsl:node-set($NodeType)/*">
    <xsl:apply-templates select="@default | @size | @value" mode="UnknownValues">
      <xsl:with-param name="ParamName" select="name()"/>
    </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="methodUnknownTypes" match="method" mode="UnknownTypes">
    <xsl:apply-templates select="@return" mode="UnknownTypes">
      <xsl:with-param name="ParamName" select="name()"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="descendant::param" mode="UnknownTypes"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="atrUnknownTypes" match="@name | @type | @return" mode="UnknownTypes">
    <xsl:param name="ParamName"/>
    <xsl:call-template name="NotifyUnknownTypes">
      <xsl:with-param name="ParamName" select="$ParamName"/>
      <xsl:with-param name="TypeName" select="."/>
    </xsl:call-template>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="NotifyUnknownTypes">
    <xsl:param name="ParamName"/>
    <xsl:param name="TypeName"/>
    <!--ICI ParamName="{$ParamName}" TypeName="{$TypeName}"/-->
    <xsl:variable name="Name">
      <xsl:call-template name="SimpleName">
        <xsl:with-param name="Name" select="$TypeName"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="Description">
      <xsl:call-template name="SimpleTypes">
        <xsl:with-param name="Label" select="$TypeName"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="SimpleName">
      <xsl:value-of select="substring-after($Name,',')"/>
    </xsl:variable>
    <xsl:variable name="Idref">
      <xsl:call-template name="ClassMember">
        <xsl:with-param name="Label" select="$SimpleName"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:if test="$Idref='' and $Description=''">
      <element-type kind="Class" node="{$ParamName}_3" import="yes">
        <xsl:attribute name="container">
          <xsl:choose>
            <xsl:when test="contains(concat($SimpleName,';'),'Collection;')">3</xsl:when>
            <xsl:when test="contains(concat($SimpleName,';'),'List;')">3</xsl:when>
            <xsl:otherwise>0</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:choose>
          <xsl:when test="contains($Name,',')">
            <xsl:attribute name="name">
              <xsl:value-of select="$SimpleName"/>
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
  <xsl:template name="vsUnknownTypes" match="@default | @value | @size" mode="UnknownValues">
    <xsl:param name="ParamName"/>
    <xsl:choose>
      <xsl:when test="string(number(.))!='NaN'"/>
      <xsl:when test="contains($VbPredefinedValues,concat(.,';'))"/>
      <xsl:when test="contains(.,'(')"/>
      <xsl:when test="starts-with(.,'&quot;')"/>
      <xsl:when test="starts-with(.,$Apos)"/>
      <xsl:otherwise>
        <xsl:variable name="Name">
          <xsl:apply-templates select="." mode="SimpleName"/>
        </xsl:variable>
        <xsl:variable name="Idref">
          <xsl:call-template name="ValueMember">
            <xsl:with-param name="Label" select="substring-after($Name,',')"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:if test="$Idref=''">
          <element-type kind="Value" node="{$ParamName}_6" container="0"
                      name="{substring-after($Name,',')}" prefix="{substring-before($Name,',')}"/>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ============================================================================== -->
  <xsl:template name="trType" match="@type | @return" mode="Type">
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
    <!--xsl:attribute name="TEST">
          <xsl:value-of select="concat(.,':=',$Idref,'|',$Description,']')"/>
    </xsl:attribute-->
    <xsl:choose>
      <xsl:when test="$Description!=''">
        <xsl:attribute name="desc">
          <xsl:value-of select="$Description"/>
        </xsl:attribute>
      </xsl:when>
      <xsl:when test="$Idref!=''">
        <xsl:variable name="Container">
          <xsl:call-template name="SearchContainer">
            <xsl:with-param name="Label" select="."/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$Container='3'">
            <xsl:attribute name="idref">
              <xsl:call-template name="SearchMember">
                <xsl:with-param name="Label">Object</xsl:with-param>
              </xsl:call-template>
            </xsl:attribute>
            <xsl:element name="list">
              <xsl:attribute name="type">simple</xsl:attribute>
              <xsl:attribute name="idref">
                <xsl:value-of select="$Idref"/>
              </xsl:attribute>
            </xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="idref">
              <xsl:value-of select="$Idref"/>
            </xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:attribute name="desc">
          <xsl:value-of select="."/>
        </xsl:attribute>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <!--xsl:template name="typeType" match="type" mode="Type">
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
  </xsl:template-->
  <!-- ======================================================================= -->
  <!--xsl:template name="variableValue" match="variable" mode="Value">
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
  </xsl:template-->
  <!-- ======================================================================= -->
  <xsl:template name="inheritedTypedef" match="inherited" mode="Typedef">
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
  <xsl:template name="inheritedList" match="inherited" mode="List">
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
  <xsl:template name="paList" match="property | attribute" mode="List">
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
  <xsl:template name="iapType" match="inherited | property | attribute" mode="Type">
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
  <xsl:template match="*" mode="Enumeration">
    <xsl:variable name="Name" select="@name"/>
    <xsl:if test="msxsl:node-set($UnknownTypes)/*[@kind='Value' and contains(@prefix,$Name)]">
      <xsl:apply-templates select="msxsl:node-set($UnknownTypes)/*[@kind='Value' and contains(@prefix,$Name)]" mode="EnumValue"/>
    </xsl:if>
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
  <xsl:template name="classSignature" match="class" mode="Signature">
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
  <xsl:template name="inheritedSignature" match="inherited" mode="Signature">
    <xsl:variable name="ClassName" select="@name"/>
    <xsl:apply-templates select="//class[@name=$ClassName]" mode="Signature"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="methodSignature" match="method" mode="Signature">
    <xsl:param name="ClassID"/>
    <signature name="{concat(@name,'(',@types,')')}">
      <xsl:copy-of select="@other"/>
      <xsl:attribute name="id">
        <xsl:value-of select="$ClassID"/>
      </xsl:attribute>
    </signature>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="classShouldInherit" match="class" mode="ShouldInherit">
    <xsl:apply-templates select="descendant::inherited" mode="Signature"/>
    <!--xsl:variable name="ShouldInherit">
    </xsl:variable>
    <xsl:for-each select="msxsl:node-set($ShouldInherit)//signature">
      <xsl:copy-of select="."/>
    </xsl:for-each-->
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="methodOverrides" match="method" mode="Overrides">
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
  <xsl:template name="modePrefixName" match="@*" mode="PrefixName">
    <xsl:variable name="Name">
      <xsl:apply-templates select="." mode="SimpleName"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="starts-with(.,$Apos)"/>
      <xsl:when test="starts-with(.,'&quot;')"/>
      <xsl:when test="contains(.,'(')"/>
      <xsl:when test="contains($Name,',')">
        <xsl:value-of select="substring-before($Name,',')"/>
      </xsl:when>
      <xsl:otherwise/>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="modeSimpleName2" match="@*" mode="SimpleName2">
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
  <xsl:template name="tnrSimpleName" match="@type | @name | @return" mode="SimpleName">
    <xsl:call-template name="SimpleName">
      <xsl:with-param name="Name" select="."/>
    </xsl:call-template>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="dvsSimpleName" match="@default | @value | @size" mode="SimpleName">
    <xsl:choose>
      <xsl:when test="starts-with(.,'&quot;')"/>
      <xsl:when test="starts-with(.,$Apos)"/>
      <xsl:when test="contains(.,'(')"/>
      <xsl:otherwise>
        <xsl:call-template name="SimpleName">
          <xsl:with-param name="Name" select="."/>
        </xsl:call-template>
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
        <xsl:text>,</xsl:text>
        <xsl:value-of select="$Name"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="modeFullpathClassName" match="*" mode="FullpathClassName">
    <xsl:if test="parent::package">
      <xsl:apply-templates select="parent::package" mode="FullpathClassName"/>
      <xsl:text>.</xsl:text>
    </xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="ClassMember">
    <xsl:param name="Label"/>
    <!--xsl:value-of select="concat('[',$Label,']=')"/-->
    <xsl:value-of select="msxsl:node-set($Classes)/*[@name=$Label and @kind='Class']/@id"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="ValueMember">
    <xsl:param name="Label"/>
    <!--xsl:value-of select="concat('[',$Label,']=')"/-->
    <xsl:value-of select="msxsl:node-set($Classes)/*[@name=$Label and @kind!='Class']/@id"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="SearchMember">
    <xsl:param name="Label"/>
    <!--xsl:value-of select="concat('[',$Label,']=')"/-->
    <xsl:value-of select="msxsl:node-set($Classes)/*[@name=$Label and @kind='Class']/@id"/>
    <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label and @kind='Class']/@idref"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="SearchContainer">
    <xsl:param name="Label"/>
    <!--xsl:value-of select="concat('[',$Label,']=')"/-->
    <xsl:value-of select="msxsl:node-set($Classes)/*[@name=$Label and @kind='Class']/@container"/>
    <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label and @kind='Class']/@container"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="SearchNode">
    <xsl:param name="Label"/>
    <!--xsl:value-of select="concat('[',$Label,']=')"/-->
    <xsl:copy-of select="msxsl:node-set($Classes)/*[@name=$Label and @kind='Class']"/>
    <xsl:copy-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label and @kind='Class']"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="SearchSize">
    <xsl:param name="Label"/>
    <xsl:variable name="Idref">
      <xsl:value-of select="msxsl:node-set($Classes)/*[@name=$Label and @kind!='Class']/@id"/>
      <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label and @kind!='Class']/@idref"/>
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
    <!--xsl:attribute name="Kind">
      <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label]/@kind"/>
    </xsl:attribute-->
    <xsl:variable name="Idref">
      <xsl:value-of select="msxsl:node-set($Classes)/*[@name=$Label and @kind='Value']/@id"/>
      <xsl:value-of select="msxsl:node-set($UnknownTypes)/*[@name=$Label and @kind='Value']/@idref"/>
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
    <xsl:call-template name="SimpleTypes">
      <xsl:with-param name="Label" select="$Label"/>
    </xsl:call-template>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="text()"/>
  <!-- ======================================================================= -->
</xsl:stylesheet>







































































