<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
  <!-- ======================================================================= -->
  <xsl:output method="xml" cdata-section-elements="code" encoding="ISO-8859-1"/>
  <!-- ======================================================================= -->
  <xsl:key match="@package" name="package" use="."/>
  <xsl:key match="reference" name="class" use="@name"/>
  <xsl:key match="reference" name="include" use="@value"/>
  <!-- ======================================================================= -->
  <xsl:param name="ProjectFolder"/>
  <xsl:param name="ToolsFolder"/>
  <xsl:param name="LanguageFolder">E:\Documents\Mes projets\uml-designer-kg-2009\ClassXmlProject\xslt</xsl:param>
  <xsl:param name="InputClass"/>
  <xsl:param name="InputPackage"/>
  <!-- ======================================================================= -->
  <xsl:variable name="Language">
    <xsl:if test="$LanguageFolder=''">
      <xsl:message terminate="yes">Parameter $LanguageFolder not yet filled!</xsl:message>
    </xsl:if>
    <xsl:value-of select="$LanguageFolder"/>
    <xsl:text>\LanguageJava.xml</xsl:text>
  </xsl:variable>
  <xsl:variable name="FileLanguage">
    <xsl:value-of select="$LanguageFolder"/>
    <xsl:text>\language.xml</xsl:text>
  </xsl:variable>
  <xsl:variable name="PrefixList" select="translate(document($FileLanguage)//PrefixList/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixTypeList" select="translate(document($FileLanguage)//PrefixTypeList/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixStructProperty" select="translate(document($FileLanguage)//PrefixStructProperty/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixEnumProperty" select="translate(document($FileLanguage)//PrefixEnumProperty/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="PrefixMember"/>
  <xsl:variable name="PrefixArray" select="translate(document($FileLanguage)//PrefixArray/text(),'&#32;&#10;&#13;','')"/>
  <xsl:variable name="SetParam" select="translate(document($FileLanguage)//SetParam/text(),'&#32;&#10;&#13;','')"/>
  <!-- ======================================================================= -->
  <xsl:template match="class">
    <xsl:element name="document">
      <xsl:apply-templates select="." mode="Code"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="package">
    <xsl:element name="document">
      <xsl:apply-templates select="." mode="Code"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef | property | method">
    <xsl:element name="document">
      No code generation at this level
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="import|relationship">
    <xsl:element name="document">
      No code generation for this node
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="/root">
    <xsl:element name="document">
      <!-- CAUTION: do not change this version, application will upgrade this for you if necessary -->
      <xsl:attribute name="version">1.0</xsl:attribute>
      <!-- Possible "method" value: code, batch -->
      <xsl:attribute name="method">code</xsl:attribute>
      <xsl:attribute name="project">
        <xsl:value-of select="$ProjectFolder"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="$InputClass!=''">
          <!--xsl:comment>InputClass=<xsl:value-of select="$InputClass"/></xsl:comment-->
          <xsl:element name="package">
            <xsl:attribute name="name">src</xsl:attribute>
            <xsl:apply-templates select="//root/package[class[@id=$InputClass]]" mode="Code"/>
          </xsl:element>
        </xsl:when>
        <xsl:when test="$InputPackage!=''">
          <!--xsl:comment>InputPackage=<xsl:value-of select="$InputPackage"/></xsl:comment-->
          <xsl:element name="package">
            <xsl:attribute name="name">src</xsl:attribute>
          <xsl:apply-templates select="//root/package[@id=$InputPackage]" mode="Code"/>
         </xsl:element>
         </xsl:when>
        <xsl:otherwise>
          <!--xsl:comment>Otherwise</xsl:comment-->
          <xsl:element name="package">
            <xsl:attribute name="name">META-INF</xsl:attribute>
            <xsl:element name="code">
      <!-- Possible "Merge" value: no, yes (To preserve your previous generated code) -->
              <xsl:attribute name="Merge">yes</xsl:attribute>
              <xsl:attribute name="name">MANIFEST.MF</xsl:attribute>
              <xsl:text>Manifest-Version: 1.0
Bundle-ManifestVersion: 2
Bundle-Name: Metamodel Plug-in
Bundle-SymbolicName:</xsl:text>
              <xsl:value-of select="concat(' ',@name)"/>
              <xsl:text>; singleton:=true
Bundle-Version: 1.0.0
Bundle-Activator: com.sodius.mdw.rhapsody.metamodel.Activator
Bundle-Vendor: SODIUS</xsl:text>
              <xsl:for-each select="import">
                <xsl:choose>
                  <xsl:when test="position()=1">
                    <xsl:text>
Require-Bundle:</xsl:text>
                    <xsl:value-of select="concat(' ',@name)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>,
</xsl:text>
                    <xsl:value-of select="concat('  ',@name)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:for-each>
              <xsl:text>
Bundle-RequiredExecutionEnvironment: JavaSE-1.6
Bundle-ActivationPolicy: lazy
</xsl:text>
            </xsl:element>
          </xsl:element>
          <xsl:element name="package">
            <xsl:attribute name="name">src</xsl:attribute>
            <xsl:apply-templates select="package" mode="Code"/>
          </xsl:element>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="package" mode="Code">
    <xsl:variable name="PackageName">
      <xsl:value-of select="@folder"/>
      <xsl:if test="not(@folder)">
        <xsl:value-of select="@name"/>
      </xsl:if>
    </xsl:variable>
    <xsl:element name="package">
      <xsl:attribute name="name">
        <xsl:value-of select="$PackageName"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="$InputClass!=''">
          <xsl:apply-templates select="class[@id=$InputClass]" mode="Code"/>
        </xsl:when>
        <xsl:when test="$InputPackage!=''">
          <xsl:if test="@id=$InputPackage">
            <xsl:apply-templates select="class" mode="Code"/>
          </xsl:if>
          <xsl:apply-templates select="package[@id=$InputPackage or descendant::package[@id=$InputPackage]]" mode="Code"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="class" mode="Code"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="Code">
    <xsl:variable name="Methods">
      <xsl:if test="@implementation!='abstract'">
        <xsl:apply-templates select="." mode="Signature"/>
      </xsl:if>
    </xsl:variable>
    <xsl:variable name="ShouldInherit">
      <xsl:if test="@implementation!='abstract'">
        <xsl:for-each select="msxsl:node-set($Methods)//signature[@implementation='abstract']">
          <xsl:variable name="Signature" select="@name"/>
          <xsl:choose>
            <xsl:when test="msxsl:node-set($Methods)//signature[@implementation!='abstract' and @name=$Signature]"/>
            <xsl:otherwise>
              <xsl:copy-of select="."/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:for-each>
      </xsl:if>
    </xsl:variable>
    <!--
    <xsl:copy-of select="$Methods"/>
    <xsl:copy-of select="$ShouldInherit"/>
    -->
    <xsl:element name="code">
      <!-- Possible "Merge" value: no, yes (To preserve your previous generated code) -->
      <xsl:attribute name="Merge">yes</xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
        <xsl:text>.java</xsl:text>
      </xsl:attribute>
      <xsl:variable name="Request1">
        <xsl:call-template name="FullImportName"/>
      </xsl:variable>
      <IMPORTS>
        <xsl:copy-of select="$Request1"/>
      </IMPORTS>
      <!--
     -->
      <xsl:variable name="CurrentPackage">
        <xsl:value-of select="parent::package/@name"/>
      </xsl:variable>
      <xsl:text/>
      <xsl:apply-templates select="import/body" mode="Declaration"/>
      <xsl:text>package </xsl:text>
      <xsl:value-of select="parent::package/@name"/>
      <xsl:text>;
</xsl:text>
      <xsl:for-each select="msxsl:node-set($Request1)//reference[generate-id()=generate-id(key('include',@value)[1])]">
        <xsl:text>
import </xsl:text>
        <xsl:value-of select="@value"/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
      <xsl:text xml:space="preserve">
</xsl:text>
      <xsl:apply-templates select="import/body" mode="Code"/>
      <xsl:text xml:space="preserve">
</xsl:text>
      <xsl:variable name="Implementation">
        <xsl:apply-templates select="@implementation" mode="Class"/>
      </xsl:variable>
      <xsl:apply-templates select="comment" mode="Comment"/>
        <xsl:text>
</xsl:text>
      <xsl:if test="@visibility='package'">
        <xsl:text>public </xsl:text>
      </xsl:if>
      <xsl:if test="msxsl:node-set($ShouldInherit)/*">abstract </xsl:if>
      <xsl:value-of select="concat($Implementation,@name)"/>
      <xsl:if test="model">
        <xsl:apply-templates select="model" mode="Class"/>
      </xsl:if>
      <xsl:apply-templates select="inherited" mode="Code">
        <xsl:sort select="id(@idref)[@root='yes' or @implementation!='abstract']" order="descending"/>
      </xsl:apply-templates>
      <xsl:text> {
</xsl:text>
    <xsl:if test="property[type/@modifier='const']"><xsl:call-template name="Constants"/></xsl:if>
      <xsl:if test="property">
        <xsl:call-template name="Properties"/>
      </xsl:if>
      <xsl:variable name="ClassID" select="@id"/>
      <xsl:if test="id(collaboration/@idref)[(child/@idref=$ClassID and father/@range!='no') or father/@idref=$ClassID]">
      <xsl:text>
    //#Region "Relationships"
    </xsl:text>
        <xsl:call-template name="Relationships">
          <xsl:with-param name="Range">private</xsl:with-param>
          <xsl:with-param name="ClassID" select="$ClassID"/>
        </xsl:call-template>
        <xsl:call-template name="Relationships">
          <xsl:with-param name="Range">protected</xsl:with-param>
          <xsl:with-param name="ClassID" select="$ClassID"/>
        </xsl:call-template>
        <xsl:call-template name="Relationships">
          <xsl:with-param name="Range">public</xsl:with-param>
          <xsl:with-param name="ClassID" select="$ClassID"/>
        </xsl:call-template><xsl:text>
    //#End Region</xsl:text>
      </xsl:if>
<xsl:text>
    //#Region "Constructors/Methods"
    </xsl:text>
  <xsl:variable name="ImplementedMethods">
  <xsl:for-each select="inherited[id(@idref)/@implementation='abstract']">
    <xsl:value-of select="concat(@idref,';')"/>
  </xsl:for-each>
  <xsl:for-each select="inherited[id(@idref)/@root='no']">
    <xsl:value-of select="concat(@idref,';')"/>
  </xsl:for-each>
  </xsl:variable>
  <xsl:call-template name="Functions">
    <xsl:with-param name="Range">private</xsl:with-param>
    <xsl:with-param name="Implementation" select="@implementation"/>
    <xsl:with-param name="ImplementedMethods" select="$ImplementedMethods"/>
  </xsl:call-template>
  <xsl:call-template name="Functions">
   <xsl:with-param name="Range">protected</xsl:with-param>
    <xsl:with-param name="Implementation" select="@implementation"/>
    <xsl:with-param name="ImplementedMethods" select="$ImplementedMethods"/>
  </xsl:call-template>
  <xsl:call-template name="Functions">
    <xsl:with-param name="Range">public</xsl:with-param>
    <xsl:with-param name="Implementation" select="@implementation"/>
    <xsl:with-param name="ImplementedMethods" select="$ImplementedMethods"/>
  </xsl:call-template>
  <xsl:text>
    //#End Region

    //#Region "Other declarations (Not managed)"
    //#End Region
}</xsl:text>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Properties">
    <xsl:if test="property">
      <xsl:text>
    //#Region "Member variables"
</xsl:text>
      <xsl:apply-templates mode="Code" select="property[@attribute='yes'][variable/@range='public'][type/@modifier!='const']">
        <xsl:sort select="@name"/>
      </xsl:apply-templates>
      <xsl:apply-templates mode="Code" select="property[@attribute='yes'][variable/@range='protected'][type/@modifier!='const']">
        <xsl:sort select="@name"/>
      </xsl:apply-templates>
      <xsl:apply-templates mode="Code" select="property[@attribute='yes'][variable/@range='private'][type/@modifier!='const']">
        <xsl:sort select="@name"/>
      </xsl:apply-templates>
      <xsl:text>
    //#End Region
</xsl:text>
    </xsl:if>
    <xsl:if test="property[variable/@range!='public'][get/@range!='no' or set/@range!='no']">
      <xsl:text>
    //#Region "Accessors"
</xsl:text>
      <xsl:call-template name="Accessors">
        <xsl:with-param name="Range">public</xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="Accessors">
        <xsl:with-param name="Range">protected</xsl:with-param>
      </xsl:call-template>
      <xsl:text>
    //#End Region
</xsl:text>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Accessors">
    <xsl:param name="Range"/>
    <xsl:apply-templates select="property[variable/@range!='public'][get/@range=$Range or set/@range=$Range]" mode="Access">
      <xsl:sort select="@name"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="property" mode="Code">
    <xsl:variable name="ConstOk">
      <xsl:if test="type/@modifier='const'">ok</xsl:if>
    </xsl:variable>
    <xsl:if test="get[@range='no'] and set[@range='no']">
      <xsl:apply-templates select="comment" mode="Simple"/>
      <xsl:text/>
    </xsl:if>
    <xsl:apply-templates select="variable/@range" mode="Code"/>
    <xsl:if test="@member='class'">static </xsl:if>
    <xsl:if test="$ConstOk='ok'">final </xsl:if>
    <xsl:apply-templates select="variable" mode="Typedef">
      <xsl:with-param name="PropertyOk">Ok</xsl:with-param>
      <xsl:with-param name="Name">
        <xsl:if test="$ConstOk!='ok'">
          <xsl:value-of select="$PrefixMember"/>
          <xsl:apply-templates select="type" mode="TypePrefix"/>
          <xsl:apply-templates select="variable" mode="TypePrefix"/>
        </xsl:if>
        <xsl:value-of select="@name"/>
      </xsl:with-param>
      <xsl:with-param name="Type">
        <xsl:choose>
          <xsl:when test="descendant::enumvalue">
            <xsl:value-of select="concat($PrefixEnumProperty,@name)"/>
          </xsl:when>
          <xsl:when test="descendant::element">
            <xsl:value-of select="concat($PrefixStructProperty,@name)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="type"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
    </xsl:apply-templates>
    <xsl:if test="variable[string-length(@value)!=0 or @valref]">
      <xsl:text> = </xsl:text>
      <xsl:value-of select="variable/@value"/>
      <xsl:apply-templates select="id(variable/@valref)" mode="FullPackageName">
        <xsl:with-param name="CurrentClassName" select="ancestor::class/@name"/>
        <xsl:with-param name="CurrentPackageName">
          <xsl:value-of  select="parent::class/parent::package/@name"/>
          <xsl:value-of  select="parent::class/parent::root/@name"/>
        </xsl:with-param>
      </xsl:apply-templates>
    </xsl:if>
    <xsl:text>;
</xsl:text>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="variable" mode="Typedef">
    <xsl:param name="PropertyOk"/>
    <xsl:param name="Type"/>
    <xsl:param name="Name"/>
    <xsl:value-of select="concat($Type, ' ', $Name)"/>
    <xsl:if test="$Type!='Sub'">
      <xsl:if test="@sizeref or @size">
        <xsl:text>[</xsl:text>
        <xsl:if test="$PropertyOk='Ok'">
          <xsl:value-of select="@size"/>
          <xsl:apply-templates select="id(@sizeref)" mode="FullPackageName">
            <xsl:with-param name="CurrentClassName" select="ancestor::class/@name"/>
            <xsl:with-param name="CurrentPackageName">
              <xsl:value-of  select="ancestor::class/parent::package/@name"/>
              <xsl:value-of  select="ancestor::class/parent::root/@name"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </xsl:if>
        <xsl:text>]</xsl:text>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@range | @constructor" mode="Code">
    <xsl:choose>
      <xsl:when test=".='private'">
        <xsl:text>
    private </xsl:text>
      </xsl:when>
      <xsl:when test=".='protected'">
        <xsl:text>
    protected </xsl:text>
      </xsl:when>
      <xsl:when test=".='public'">
        <xsl:text>
    public </xsl:text>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@implementation" mode="Class">
    <xsl:choose>
      <xsl:when test=".='abstract'">interface </xsl:when>
      <xsl:otherwise>class </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@implementation" mode="Code">
    <xsl:param name="Implementation"/>
    <xsl:choose>
      <xsl:when test=".='final'">final </xsl:when>
      <xsl:when test=".='abstract' and $Implementation!='abstract'">abstract </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited" mode="Code">
    <xsl:apply-templates select="." mode="Inherit"/>
    <xsl:apply-templates select="id(@idref)" mode="FullPackageName"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited" mode="Inherit">
    <xsl:choose>
      <xsl:when test="id(@idref)[@implementation='abstract' or @root='no']">
        <xsl:text>
                  implements </xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>
                  extends </xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="comment" mode="Comment">
    <xsl:variable name="Brief">
      <xsl:if test="string-length(@brief)=0">
        <xsl:value-of select="text()"/>
      </xsl:if>
      <xsl:value-of select="@brief"/>
    </xsl:variable>
    <xsl:variable name="Detail">
      <xsl:if test="string-length(@brief)&gt;0">
        <xsl:value-of select="text()"/>
      </xsl:if>
    </xsl:variable>
    <xsl:text>
    
    /**
      * </xsl:text>
    <xsl:value-of select="$Brief"/>
    <xsl:text>
      * &lt;p&gt;</xsl:text>
    <xsl:value-of select="$Detail"/>
    <xsl:text>&lt;/p&gt;
    **/</xsl:text>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="comment" mode="Simple">
    <xsl:text>
    // </xsl:text>
    <xsl:value-of select="text()"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="FullImportName">
    <xsl:variable name="ClassID" select="@id"/>
    <xsl:variable name="CurrentPackageName">
      <xsl:value-of  select="parent::package/@name"/>
      <xsl:value-of  select="parent::root/@name"/>
    </xsl:variable>
    <xsl:apply-templates select="descendant::*/@desc" mode="ListImports"/>
    <xsl:apply-templates select="descendant::*/@index-desc" mode="ListImports"/>
    <xsl:apply-templates select="import[not(export or body)]" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackageName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="inherited" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackageName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="dependency[@type='interface' or @type='reference']" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackageName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="//relationship/father[@idref=$ClassID]" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackageName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="//relationship/child[@idref=$ClassID]" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackageName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="typedef" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackageName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="property" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackageName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="method/return" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackageName"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="method/param" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackageName"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef | property | param | return" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:choose>
      <xsl:when test="type/element[@idref]">
        <xsl:if test="id(type/element/@idref)">
          <xsl:apply-templates select="type/element[@idref]" mode="ListImports">
            <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
          </xsl:apply-templates>
        </xsl:if>
      </xsl:when>
      <xsl:when test="type[@idref and not(enumvalue)]">
        <xsl:if test="id(type/@idref)">
          <xsl:apply-templates select="type[@idref]" mode="ListImports">
            <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
          </xsl:apply-templates>
        </xsl:if>
      </xsl:when>
    </xsl:choose>
    <xsl:if test="type/list[@idref]">
      <xsl:if test="id(type/list/@idref)">
        <xsl:apply-templates select="type/list[@idref]" mode="ListImports">
          <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
        </xsl:apply-templates>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type | element | @index-idref" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:variable name="RefID">
      <xsl:value-of select="@idref"/>
      <xsl:if test="name()='index-idref'">
        <xsl:value-of select="."/>
      </xsl:if>
    </xsl:variable>
    <xsl:if test="id($RefID)">
      <xsl:apply-templates select="id($RefID)" mode="ClassImports">
        <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class | reference | interface" mode="ClassImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:apply-templates select="." mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:if test="id(following-sibling::father/@idref)">
      <xsl:apply-templates select="id(preceding-sibling::father/@idref)" mode="ListImports">
        <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
      </xsl:apply-templates>
    </xsl:if>
    <xsl:apply-templates select="preceding-sibling::father/list" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="father" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:if test="id(following-sibling::child/@idref)">
      <xsl:apply-templates select="id(following-sibling::child/@idref)" mode="ListImports">
        <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
      </xsl:apply-templates>
    </xsl:if>
    <xsl:apply-templates select="following-sibling::child/list" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="list" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:if test="id(@index-idref)">
      <xsl:apply-templates select="id(@index-idref)" mode="ListImports">
        <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
      </xsl:apply-templates>
    </xsl:if>
    <xsl:if test="id(@idref)">
      <xsl:apply-templates select="id(@idref)" mode="ListImports">
        <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:if test="id(@idref)">
      <xsl:apply-templates select="id(@idref)" mode="ListImports">
        <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="dependency" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <reference name="dependency" ref="{id(@idref)/@name}"/>
    <xsl:apply-templates select="id(@idref)" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ===================================================================== -->
  <xsl:template match="type" mode="TypeImports">
      <xsl:element name="reference">
        <xsl:attribute name="node">type</xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="@implementation"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="@import"/>
        </xsl:attribute>
        <xsl:attribute name="value">
          <xsl:value-of select="concat(@import,'.',@implementation)"/>
        </xsl:attribute>
        <xsl:copy-of select="."/>
      </xsl:element>
  </xsl:template>
  <!-- ===================================================================== -->
  <xsl:template match="@desc | @index-desc" mode="ListImports">
    <xsl:variable name="Type" select="."/>
    <xsl:apply-templates select="document($Language)//type[@name=$Type][@import!='' and @import!='java.lang']" mode="TypeImports"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference | interface" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:variable name="TargetPackage">
      <xsl:choose>
        <xsl:when test="string-length(@package)!=0">
          <xsl:value-of select="@package"/>
        </xsl:when>
        <xsl:when test="string-length(ancestor::import/@param)!=0">
          <xsl:value-of select="ancestor::import/@param"/>
        </xsl:when>
        <xsl:otherwise>UnknownPackage</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="$CurrentPackage!=$TargetPackage">
      <xsl:element name="reference">
        <xsl:attribute name="node">
          <xsl:value-of select="name()"/>
        </xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="$TargetPackage"/>
        </xsl:attribute>
        <xsl:attribute name="value">
          <xsl:apply-templates select="." mode="Package"/>
          <xsl:text>.</xsl:text>
          <xsl:value-of select="@name"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:element name="reference">
      <xsl:attribute name="node">
        <xsl:value-of select="name()"/>
      </xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="package">
        <xsl:value-of select="parent::package/@name"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:apply-templates select="." mode="Package"/>
        <xsl:text>.</xsl:text>
        <xsl:value-of select="@name"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="Package2">
    <xsl:apply-templates select="parent::package" mode="Package2"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="Package">
    <xsl:apply-templates select="parent::package" mode="Package"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="package" mode="Package2">
    <xsl:if test="parent::package">
      <xsl:apply-templates select="parent::package" mode="Package2"/>
      <xsl:text>.</xsl:text>
    </xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="package" mode="Package">
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="root" mode="Package">
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference | interface" mode="Package">
    <xsl:value-of select="ancestor::import/@param"/>
    <xsl:if test="string-length(ancestor::import/@param)!=0 and string-length(@package)!=0">.</xsl:if>
    <xsl:value-of select="@package"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type">
    <xsl:choose>
      <xsl:when test="enumvalue">
        <xsl:text>enum</xsl:text>
        <xsl:if test="parent::property">
          <xsl:value-of select="concat(' ', PrefixEnumProperty, parent::property/@name)"/>
        </xsl:if>
        <xsl:apply-templates select="enumvalue" mode="Code"/>
      </xsl:when>
      <xsl:when test="element">
        <xsl:value-of select="@struct"/>
        <xsl:if test="parent::property">
          <xsl:value-of select="concat(' ', $PrefixStructProperty, parent::property/@name)"/>
        </xsl:if>
        <xsl:apply-templates select="element"/>
      </xsl:when>
      <xsl:when test="list or array">
        <xsl:apply-templates select="list | array" mode="Typedef">
          <xsl:with-param name="Type">
            <xsl:apply-templates select="@desc"/>
            <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
              <xsl:with-param name="CurrentClassName">
                <xsl:if test="@idref!=ancestor::class/@id">
                  <xsl:value-of select="ancestor::class/@name"/>
                </xsl:if>
              </xsl:with-param>
              <xsl:with-param name="CurrentPackageName">
                <xsl:value-of  select="ancestor::class/parent::package/@name"/>
                <xsl:value-of  select="ancestor::class/parent::root/@name"/>
              </xsl:with-param>
            </xsl:apply-templates>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="@desc"/>
        <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
          <xsl:with-param name="CurrentClassName">
            <xsl:if test="@idref!=ancestor::class/@id">
              <xsl:value-of select="ancestor::class/@name"/>
            </xsl:if>
          </xsl:with-param>
          <xsl:with-param name="CurrentPackageName">
            <xsl:value-of  select="ancestor::class/parent::package/@name"/>
            <xsl:value-of  select="ancestor::class/parent::root/@name"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="list" mode="Typedef">
    <xsl:param name="Type"/>
    <xsl:variable name="Index">
      <xsl:apply-templates select="@index-desc"/>
      <xsl:apply-templates select="id(@index-idref)" mode="FullPackageName">
        <xsl:with-param name="CurrentPackageName">
          <xsl:value-of  select="ancestor::class/parent::package/@name"/>
          <xsl:value-of  select="ancestor::class/parent::root/@name"/>
        </xsl:with-param>
      </xsl:apply-templates>
    </xsl:variable>
    <xsl:value-of select="@desc"/>
    <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
      <xsl:with-param name="CurrentPackageName">
        <xsl:value-of  select="ancestor::class/parent::package/@name"/>
        <xsl:value-of  select="ancestor::class/parent::root/@name"/>
      </xsl:with-param>
    </xsl:apply-templates>
    <xsl:text>(Of </xsl:text>
    <xsl:if test="@type='indexed'">
      <xsl:value-of select="$Index"/>,
    </xsl:if>
    <xsl:value-of select="$Type"/>
    <xsl:text>)</xsl:text>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="array" mode="Typedef">
    <xsl:param name="Type"/>
    <xsl:value-of select="$Type"/>
    <xsl:if test="@size">[
      <xsl:value-of select="@size"/>]
    </xsl:if>
    <xsl:if test="@sizeref">[
      <xsl:apply-templates select="id(@sizeref)" mode="FullPackageName"/>]
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@desc | @index | @index-desc">
    <xsl:call-template name="PredefinedType">
      <xsl:with-param name="Type" select="."/>
    </xsl:call-template>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="PredefinedType">
    <xsl:param name="Type"/>
    <xsl:variable name="FinalType">
      <xsl:value-of select="document($Language)//type[@name=$Type]/@implementation"/>
    </xsl:variable>
    <xsl:value-of select="$FinalType"/>
    <xsl:if test="string-length($FinalType)=0">
      <xsl:value-of select="$Type"/>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="model" mode="FullPackageName">
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child | father" mode="FullPackageName">
    <xsl:param name="CurrentClassName"/>
    <xsl:param name="CurrentPackageName"/>
    <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
      <xsl:with-param name="CurrentClassName" select="$CurrentClassName"/>
      <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef" mode="FullPackageName">
    <xsl:param name="CurrentClassName"/>
    <xsl:param name="CurrentPackageName"/>
    <!--xsl:value-of select="concat('typedef=',@name,'|',parent::class/parent::package/@name,'::',parent::class/@name)"/>
    <xsl:value-of select="concat($CurrentPackageName,'::',$CurrentClassName)"/-->
    <xsl:variable name="Class">
      <xsl:apply-templates select="parent::class" mode="FullPackageName">
        <xsl:with-param name="CurrentClassName" select="$CurrentClassName"/>
        <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
      </xsl:apply-templates>
    </xsl:variable>
    <xsl:if test="string-length($Class)!=0">
      <xsl:value-of select="concat($Class,'.')"/>
    </xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="FullPackageName">
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="enumvalue" mode="FullPackageName">
    <xsl:value-of select="concat(ancestor::class/@name, '.')"/>
    <xsl:value-of select="concat(ancestor::typedef/@name,'.',@name)"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference |interface" mode="FullPackageName">
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type" mode="Access">
    <xsl:choose>
      <xsl:when test="enumvalue and parent::property">
        <xsl:value-of select="concat($PrefixEnumProperty,parent::property/@name)"/>
      </xsl:when>
      <xsl:when test="element and parent::property">
        <xsl:value-of select="concat($PrefixStructProperty,parent::property/@name)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="."/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="property" mode="Access">
    <xsl:variable name="ClassImpl" select="parent::class/@implementation"/>
    <xsl:variable name="InheritedClassImpl">
      <xsl:choose>
        <xsl:when test="not(@overrides)"/>
        <xsl:when test="id(@overrides)[self::interface[@root='yes']]">root</xsl:when>
        <xsl:when test="id(@overrides)[self::interface]">abstract</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="id(@overrides)/@implementation"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="InheritedClassName" select="id(@overrides)/@name"/>
    <xsl:variable name="VarName">
      <xsl:value-of select="$PrefixMember"/>
      <xsl:apply-templates select="type" mode="TypePrefix"/>
      <xsl:apply-templates select="variable" mode="TypePrefix"/>
      <xsl:value-of select="@name"/>
    </xsl:variable>
    <xsl:variable name="Type">
      <xsl:choose>
        <xsl:when test="variable[@size or @sizeref]">
          <xsl:apply-templates select="type" mode="Access"/>
          <xsl:text>()</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="type" mode="Access"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Range">
      <xsl:choose>
        <xsl:when test="get/@range!='no'">
          <xsl:apply-templates select="get/@range" mode="Code"/>
        </xsl:when>
        <xsl:when test="set/@range!='no'">
          <xsl:apply-templates select="set/@range" mode="Code"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:apply-templates select="comment" mode="Simple"/>
    <xsl:text/>
    <xsl:variable name="Name">
      <xsl:apply-templates select="@name" mode="Accessor"/>
    </xsl:variable>
    <xsl:if test="get[@range!='no']">
      <xsl:value-of select="$Range"/>
      <xsl:if test="@overridable='no' and @overrides!='' and $InheritedClassImpl!='abstract'">final </xsl:if>
      <xsl:if test="@member='class'">static </xsl:if>
      <xsl:value-of select="concat($Type,' get',$Name,'()')"/>
      <xsl:if test="$ClassImpl!='abstract'">
        <xsl:text> {</xsl:text>
        <xsl:if test="@attribute='yes'">
          <xsl:if test="get/@inline='no' and @attribute='yes'">
            <xsl:text>
        return </xsl:text>
            <xsl:value-of select="$VarName"/>
            <xsl:text>;</xsl:text>
          </xsl:if>
        </xsl:if>
        <xsl:text>
    }
    </xsl:text>
      </xsl:if>
    </xsl:if>
    <xsl:if test="set[@range!='no']">
      <xsl:value-of select="$Range"/>
      <xsl:if test="@overridable='no' and @overrides!='' and $InheritedClassImpl!='abstract'">final </xsl:if>
      <xsl:if test="@member='class'">static </xsl:if>
      <xsl:value-of select="concat('void set',$Name,'(',$Type,' ',$SetParam,')')"/>
      <xsl:if test="$ClassImpl!='abstract'">
        <xsl:text> {
        </xsl:text>
        <xsl:if test="set/@inline='no' and @attribute='yes'">
          <xsl:value-of select="concat($VarName,' = ',$SetParam)"/>
          <xsl:text>;</xsl:text>
        </xsl:if>
        <xsl:text>
    }</xsl:text>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child | father" mode="Access">
    <xsl:param name="CurrentPackageName"/>
    <xsl:param name="CurrentClassID"/>
    <xsl:variable name="Type">
      <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
        <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
      </xsl:apply-templates>
    </xsl:variable>
    <xsl:variable name="Name">
      <xsl:apply-templates select="@name" mode="Accessor"/>
    </xsl:variable>
    <xsl:if test="get[@range!='no']">
    <xsl:text>
    public </xsl:text>
    <xsl:if test="@member='class'">static </xsl:if>
    <xsl:value-of select="concat($Type,' get',$Name,'()')"/>
      <xsl:text> {
        return </xsl:text>
      <xsl:value-of select="concat($PrefixMember,@name)"/>
      <xsl:text>;
    }
        </xsl:text>
    </xsl:if>
    <xsl:if test="set[@range!='no']">
        <xsl:text>
    public </xsl:text>
    <xsl:if test="@member='class'">static </xsl:if>
    <xsl:value-of select="concat('void set',$Name)"/>
      <xsl:value-of select="concat('(',$Type,' ',$SetParam,')')"/>
      <xsl:text> {
        </xsl:text>
      <xsl:value-of select="concat($PrefixMember,$Name,' = ',$SetParam)"/>
      <xsl:text>;
    }
</xsl:text>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Relationships">
    <xsl:param name="Range"/>
    <xsl:param name="ClassID"/>
    <xsl:variable name="CurrentPackageName" select="id($ClassID)/parent::package/@name"/>
    <xsl:if test="//relationship[($Range= child/@range and father/@idref=$ClassID) or ($Range= father/@range and child/@idref=$ClassID) or                   (($Range= child/get/@range or $Range= child/set/@range) and father/@idref=$ClassID) or (($Range= father/get/@range or                   $Range= father/set/@range) and child/@idref=$ClassID)]">
      <xsl:apply-templates select="//relationship/child[$Range= @range and preceding-sibling::father/@idref=$ClassID]" mode="Relation">
        <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
        <xsl:with-param name="CurrentClassID" select="$ClassID"/>
      </xsl:apply-templates>
      <xsl:apply-templates select="//relationship/father[$Range= @range and following-sibling::child/@idref=$ClassID]" mode="Relation">
        <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
        <xsl:with-param name="CurrentClassID" select="$ClassID"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child | father" mode="Relation">
    <xsl:param name="CurrentPackageName"/>
    <xsl:param name="CurrentClassID"/>
    <xsl:call-template name="RelationMember">
      <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
      <xsl:with-param name="CurrentClassID" select="$CurrentClassID"/>
      <xsl:with-param name="Comment">
        <xsl:value-of select="concat('Relation &quot;',parent::*/@action,'&quot; to  ',@cardinal,' ',id(@idref)/@name)"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="RelationMember">
    <xsl:param name="CurrentPackageName"/>
    <xsl:param name="CurrentClassID"/>
    <xsl:param name="Comment"/>
    <xsl:choose>
      <xsl:when test="@cardinal='01' or @cardinal='1'">
          <xsl:text>
    // </xsl:text>
          <xsl:value-of select="$Comment"/>
        <xsl:apply-templates select="@range" mode="Code"/>
        <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
          <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
        </xsl:apply-templates>
        <xsl:text xml:space="preserve"> </xsl:text>
        <xsl:value-of select="concat($PrefixMember,@name)"/>
        <xsl:text>;
    </xsl:text>
        <xsl:apply-templates select="." mode="Access">
          <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
          <xsl:with-param name="CurrentClassID" select="$CurrentClassID"/>
          <xsl:with-param name="Comment" select="$Comment"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="*">
          <xsl:with-param name="Range">
            <xsl:apply-templates select="@range" mode="Code"/>
          </xsl:with-param>
          <xsl:with-param name="ClassName">
            <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
              <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
            </xsl:apply-templates>
          </xsl:with-param>
          <xsl:with-param name="MemberName" select="@name"/>
          <xsl:with-param name="CurrentClassID" select="$CurrentClassID"/>
          <xsl:with-param name="Comment" select="$Comment"/>
        </xsl:apply-templates>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@name" mode="Accessor">
    <xsl:value-of select="translate(substring(.,1,1),'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
    <xsl:value-of select="substring(.,2)"/>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Constants">
<xsl:text>
    //#Region "Constants"
</xsl:text>
    <xsl:apply-templates mode="Code" select="property[type/@modifier='const']">
      <xsl:sort select="@name"/>
    </xsl:apply-templates>
<xsl:text>
    //#End Region
</xsl:text>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Functions">
    <xsl:param name="Range"/>
    <xsl:param name="Implementation"/>
    <xsl:param name="ImplementedMethods"/>
    <xsl:if test="@constructor=$Range or method[return/variable/@range=$Range or @constructor=$Range]">
      <xsl:apply-templates select="@constructor[.=$Range]"/>
      <xsl:apply-templates select="method[return/variable/@range=$Range or @constructor=$Range]" mode="Code">
        <xsl:with-param name="Implementation" select="$Implementation"/>
        <xsl:with-param name="ImplementedMethods" select="$ImplementedMethods"/>
        <xsl:sort select="@name"/>
      </xsl:apply-templates>
      <xsl:apply-templates select="@destructor[.=$Range]"/>
    </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="@constructor">
<xsl:text>
    /*
    * Default constructor
    */</xsl:text>
    <xsl:apply-templates select="." mode="Code"/>
    <xsl:value-of select="parent::class/@name"/>
    <xsl:text>() {
    }
    </xsl:text></xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="method" mode="Code">
    <xsl:param name="Implementation"/>
    <xsl:param name="ImplementedMethods"/>
    <xsl:call-template name="Method">
      <xsl:with-param name="Implementation" select="$Implementation"/>
      <xsl:with-param name="ImplementedMethods" select="$ImplementedMethods"/>
    </xsl:call-template>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Method">
    <xsl:param name="Implementation"/>
    <xsl:param name="ImplementedMethods"/>
    <xsl:variable name="TemplateOk"><xsl:if test="parent::class/@implementation='container'">ok</xsl:if></xsl:variable>
    <xsl:variable name="MethodName">
	  <xsl:choose>
	    <xsl:when test="@constructor!='no'"><xsl:apply-templates select="@constructor" mode="Code"/>
	    <xsl:value-of select="parent::class/@name"/>
        </xsl:when>
      <xsl:when test="@name='operator'"><xsl:value-of select="@operator"/></xsl:when>
      <xsl:otherwise><xsl:value-of select="@name"/></xsl:otherwise>
	  </xsl:choose>
    </xsl:variable>
    <xsl:if test="@constructor!='no'">
      <xsl:apply-templates select="." mode="Comment"/>
	  <xsl:text>
    </xsl:text>
      <xsl:value-of select="concat($MethodName,'(')"/>
      <xsl:apply-templates select="param" mode="Code">
        <xsl:with-param name="Range" select="@constructor"/>
      </xsl:apply-templates>
      <xsl:text>) {
    }</xsl:text>
    </xsl:if>
    <xsl:if test="@constructor='no'">
      <xsl:variable name="Type">
        <xsl:apply-templates select="return/type"/>
      </xsl:variable>
      <xsl:variable name="Prefix">
        <xsl:choose>
          <xsl:when test="$Type='Sub'">void </xsl:when>
          <xsl:when test="$Type!='Sub' and @name='operator'">Operator </xsl:when>
        </xsl:choose>
      </xsl:variable>
      <xsl:apply-templates select="." mode="Comment"/>
	  <xsl:text>
    </xsl:text>
      <xsl:if test="$Implementation!='abstract'">
        <xsl:apply-templates select="return/variable/@range" mode="Code"/>
      </xsl:if>
      <xsl:if test="not(@overrides) or not(contains($ImplementedMethods,concat(@overrides,';')))">
        <xsl:apply-templates select="@implementation" mode="Code">
          <xsl:with-param name="Implementation" select="$Implementation"/>
        </xsl:apply-templates>
      </xsl:if>
 	  <xsl:if test="@behaviour"><xsl:value-of select="concat(@behaviour,' ')"/></xsl:if>
      <xsl:if test="@member='class'">static </xsl:if>
      <xsl:value-of select="$Prefix"/>
      <xsl:apply-templates select="return/variable" mode="Typedef">
        <xsl:with-param name="Name">
          <xsl:value-of select="$MethodName"/>
          <xsl:text>(</xsl:text>
          <xsl:apply-templates select="param" mode="Code">
            <xsl:with-param name="Range" select="return/variable/@range"/>
          </xsl:apply-templates>
          <xsl:text>)</xsl:text>
          <xsl:choose>
          <xsl:when test="@implementation!='abstract'"><xsl:text> {
          </xsl:text></xsl:when>
          <xsl:otherwise><xsl:text>;</xsl:text></xsl:otherwise>
          </xsl:choose>
        </xsl:with-param>
        <xsl:with-param name="Type" select="$Type"/>
      </xsl:apply-templates>
      <xsl:if test="@overrides">
      </xsl:if>
      <xsl:if test="@implementation!='abstract'"><xsl:text>
    }
    </xsl:text>
    <xsl:value-of select="$Prefix"/>
      </xsl:if>
	  <xsl:text>
    </xsl:text>
   </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="param" mode="Code">
    <xsl:variable name="By">
	  <!--xsl:choose>
	    <xsl:when test="type/@by='val'">ByVal </xsl:when>
  	    <xsl:when test="type/@by='ref'">ByRef </xsl:when>
	  </xsl:choose-->
	</xsl:variable>
    <xsl:if test="variable[string-length(@value)!=0 or @valref]">Optional </xsl:if>
    <xsl:apply-templates select="variable" mode="Typedef">
      <xsl:with-param name="Name" select="concat($By,@name)"/>
      <xsl:with-param name="Type">
        <xsl:apply-templates select="type"/>
      </xsl:with-param>
    </xsl:apply-templates>
    <xsl:if test="variable[string-length(@value)!=0 or @valref]">
      <xsl:value-of select="concat(' = ',variable/@value)"/>
      <xsl:apply-templates select="id(variable/@valref)" mode="FullPackageName">
        <xsl:with-param name="CurrentClassName" select="ancestor::class/@name"/>
        <xsl:with-param name="CurrentPackageName">
          <xsl:value-of  select="ancestor::class/parent::package/@name"/>
          <xsl:value-of  select="ancestor::class/parent::root/@name"/>
        </xsl:with-param>
      </xsl:apply-templates>
    </xsl:if>
    <xsl:if test="position()!=last()">, </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="Signature">
    <xsl:copy>
      <xsl:copy-of select="@name"/>
      <xsl:copy-of select="@implementation"/>
      <xsl:apply-templates select="method[id(@overrides)[@implementation='abstract' or @implementation='root' or @implementation='virtual']]" mode="Signature"/>
      <xsl:apply-templates select="method[@implementation='abstract']" mode="Signature"/>
      <xsl:apply-templates select="id(inherited/@idref)" mode="Signature"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="method" mode="Signature">
    <xsl:variable name="Signature">
      <xsl:value-of select="@name"/>
      <xsl:text>(</xsl:text>
      <xsl:for-each select="param">
        <xsl:if test="position()!=1">,</xsl:if>
        <xsl:value-of select="concat(type/@desc,type/@idref)"/>
      </xsl:for-each>
      <xsl:text>)</xsl:text>
    </xsl:variable>
    <signature name="{$Signature}">
      <xsl:copy-of select="@implementation"/>
      <xsl:if test="@overrides">
        <xsl:attribute name="overrides">
          <xsl:value-of select="@overrides"/>
        </xsl:attribute>
      </xsl:if>
    </signature>
  </xsl:template>
    <!-- ======================================================================= -->
  </xsl:stylesheet>




