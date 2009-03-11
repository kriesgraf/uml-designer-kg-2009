<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
<!-- ======================================================================= -->
  <xsl:output method="xml" cdata-section-elements="code" encoding="ISO-8859-1"/>
<!-- ======================================================================= -->
  <xsl:param name="UmlFolder"/>
  <xsl:param name="InputClass"/>
  <xsl:param name="InputPackage"/>
  <!-- ======================================================================= -->
  <xsl:variable name="FileLanguage"><xsl:value-of select="$UmlFolder"/>\language.xml</xsl:variable>
  <xsl:variable name="PrefixList" select="document($FileLanguage)//PrefixList/text()"/>
  <xsl:variable name="PrefixMember" select="document($FileLanguage)//PrefixMember/text()"/>
  <xsl:variable name="PrefixArray" select="document($FileLanguage)//PrefixArray/text()"/>
  <xsl:variable name="TypeAccess" select="document($FileLanguage)//TypeAccess/text()"/>
  <xsl:variable name="SetParam">value</xsl:variable>
<!-- ======================================================================= -->
  <xsl:include href="vbnet-types.xsl"/>
<!-- ======================================================================= -->
  <xsl:key match="@package" name="package" use="."/>
  <xsl:key match="reference" name="class" use="@name"/>
  <xsl:key match="reference" name="include" use="@value"/>
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
  <xsl:template match="/typedef">
    <xsl:element name="document">
        No code generation at this level
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
    <xsl:choose>
      <xsl:when test="$InputClass!=''">
        <xsl:apply-templates select="//root/package[descendant::class[@id=$InputClass]]" mode="Code"/>
        <xsl:apply-templates select="//root/class[@id=$InputClass]" mode="Code"/>
      </xsl:when>
      <xsl:when test="$InputPackage!=''">
        <xsl:apply-templates select="//root/package[@id=$InputPackage or descendant::package[@id=$InputPackage]]" mode="Code"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="class" mode="Code"/>
        <xsl:apply-templates select="package" mode="Code"/>
      </xsl:otherwise>
    </xsl:choose>
    </xsl:element>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="package" mode="Code">
    <xsl:variable name="PackageName"><xsl:value-of select="@folder"/><xsl:if test="not(@folder)"><xsl:value-of select="@name"/></xsl:if></xsl:variable>
    <package name="{$PackageName}">
      <xsl:choose>
        <xsl:when test="$InputClass!=''">
          <xsl:apply-templates select="package[descendant::class[@id=$InputClass]]" mode="Code"/>
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
        <xsl:apply-templates select="package" mode="Code"/>
        </xsl:otherwise>
      </xsl:choose>
    </package>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="class" mode="Code">
    <xsl:element name="code">
      <xsl:attribute name="name"><xsl:value-of select="@name"/>.vb</xsl:attribute>
    <xsl:variable name="Request1"><xsl:call-template name="FullImportName"/></xsl:variable>
    <IMPORTS><xsl:copy-of select="$Request1"/></IMPORTS>
    <!--
     -->
    <xsl:variable name="CurrentPackage"><xsl:value-of select="parent::package/@name"/></xsl:variable>
    <xsl:for-each select="msxsl:node-set($Request1)//reference[generate-id()=generate-id(key('include',@value)[1])]">
Imports <xsl:value-of select="@value"/></xsl:for-each>
<xsl:text xml:space="preserve">
</xsl:text>
    <xsl:apply-templates select="import/body" mode="Code"/>
<xsl:text xml:space="preserve">
</xsl:text>
    <xsl:if test="parent::package">
    <xsl:text xml:space="preserve">
</xsl:text>
    <xsl:apply-templates select="import/body" mode="Declaration"/>
Namespace <xsl:value-of select="parent::package/@name"/>
    </xsl:if>
    <xsl:variable name="Implementation">
      <xsl:apply-templates select="@implementation" mode="Class"/>
    </xsl:variable>
    <xsl:apply-templates select="comment" mode="Comment"/>
    <xsl:if test="parent::package">
    Public </xsl:if>
    <xsl:if test="not(parent::package)">
Public </xsl:if>
	<xsl:if test="@behaviour"><xsl:value-of select="concat(@behaviour,' ')"/> </xsl:if>
    <xsl:if test="@implementation='final'">NotInheritable </xsl:if>
    <xsl:if test="@implementation!='abstract' and method[@implementation='abstract']">MustInherit </xsl:if>
    <xsl:value-of select="concat($Implementation,@name)"/>
    <xsl:if test="model"><xsl:apply-templates select="model" mode="Class"/></xsl:if>
    <xsl:apply-templates select="inherited" mode="Code"/>
    <xsl:if test="property[type/@modifier='const']"><xsl:call-template name="Constants"/></xsl:if>
    <xsl:if test="typedef"><xsl:call-template name="Typedef"/></xsl:if>
  <xsl:if test="property"><xsl:call-template name="Properties"/></xsl:if>
  <xsl:variable name="ClassID" select="@id"/>
  <xsl:if test="id(collaboration/@idref)[(child/@idref=$ClassID and father/@range!='no') or father/@idref=$ClassID]">
#Region "Relationships"
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
    </xsl:call-template>
#End Region
  </xsl:if>

#Region "Constructors/Methods"
  <xsl:variable name="ImplementedMethods">
  <xsl:for-each select="inherited[id(@idref)/@implementation='abstract']">
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
#End Region

<xsl:if test="not(parent::package)">
End <xsl:apply-templates select="@implementation" mode="Class"/></xsl:if>
    <xsl:if test="parent::package">
    End <xsl:apply-templates select="@implementation" mode="Class"/>
End Namespace
    </xsl:if>
    </xsl:element>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Typedef">

#Region "Predefined types"
    <xsl:if test="typedef[variable/@range='public']">
    <xsl:apply-templates select="typedef[variable/@range='public']" mode="Code"/>
    </xsl:if>
    <xsl:if test="typedef[variable/@range='protected']">

    <xsl:apply-templates select="typedef[variable/@range='protected']" mode="Code"/>
    </xsl:if>
    <xsl:if test="typedef[variable/@range='private']">

    <xsl:apply-templates select="typedef[variable/@range='private']" mode="Code"/>
    </xsl:if>
#End Region
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="model" mode="Class">
    <xsl:if test="position()=1">(Of </xsl:if>
    <xsl:value-of select="@name"/>
    <xsl:if test="position()!=last()">, </xsl:if>
    <xsl:if test="position()=last()">)</xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="inherited" mode="Code">
    <xsl:apply-templates select="." mode="Inherit"/>
    <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
      <xsl:with-param name="CurrentPackageName" select="parent::class/parent::package/@name"/>
    </xsl:apply-templates>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="inherited" mode="Inherit">
  <xsl:choose>
    <xsl:when test="id(@idref)/@implementation='abstract'">
    Implements </xsl:when>
    <xsl:otherwise>
    Inherits </xsl:otherwise>
  </xsl:choose>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="@implementation" mode="Class">
  <xsl:choose>
    <xsl:when test=".='abstract'">Interface </xsl:when>
    <xsl:otherwise>Class </xsl:otherwise>
  </xsl:choose>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Constants">
  
#Region "Constants"
    <xsl:apply-templates mode="Code" select="property[type/@modifier='const']">
      <xsl:sort select="@name"/>
    </xsl:apply-templates>
#End Region
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Properties">
    <xsl:if test="property">

#Region "Member variables"

    <xsl:apply-templates mode="Code" select="property[variable/@range='public'][type/@modifier!='const']">
      <xsl:sort select="@name"/></xsl:apply-templates>
    <xsl:apply-templates mode="Code" select="property[variable/@range='protected'][type/@modifier!='const']">
      <xsl:sort select="@name"/></xsl:apply-templates>
    <xsl:apply-templates mode="Code" select="property[variable/@range='private'][type/@modifier!='const']">
      <xsl:sort select="@name"/></xsl:apply-templates>
#End Region
    </xsl:if>
	<xsl:if test="property[variable/@range!='public'][get/@range!='no' or set/@range!='no']">
#Region "Properties"
        <xsl:call-template name="Accessors">
        <xsl:with-param name="Range">public</xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="Accessors">
        <xsl:with-param name="Range">protected</xsl:with-param>
        </xsl:call-template>
#End Region
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
  <xsl:template name="Relationships">
    <xsl:param name="Range"/>
    <xsl:param name="ClassID"/>
    <xsl:variable name="CurrentPackageName" select="id($ClassID)/parent::package/@name"/>
    <xsl:if test="//relationship[($Range= child/@range and father/@idref=$ClassID) or ($Range= father/@range and child/@idref=$ClassID) or                  (($Range= child/get/@range or $Range= child/set/@range) and father/@idref=$ClassID) or (($Range= father/get/@range or                  $Range= father/set/@range) and child/@idref=$ClassID)]">
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
  <xsl:template match="@constructor">

    ''' &lt;summary&gt;
    ''' Default constructor
    ''' &lt;/summary&gt;
    <xsl:apply-templates select="." mode="Code"/> Sub New()
    End Sub</xsl:template>
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
    </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="typedef" mode="Code">
    <xsl:apply-templates select="comment" mode="Simple"/>
    <xsl:text>
    </xsl:text>
    <xsl:apply-templates select="variable/@range" mode="Code"/>
    <xsl:choose>
      <xsl:when test="descendant::enumvalue">
        <xsl:value-of select="concat('Enum ',@name)"/>
        <xsl:apply-templates select="descendant::enumvalue" mode="Code"/>
    End Enum
      </xsl:when>
      <xsl:when test="descendant::element">
        <xsl:value-of select="concat('Structure ',@name)"/>
        <xsl:apply-templates select="descendant::element" mode="Code"/>
    End Structure
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat('Class ',@name)"/>
        Inherits <xsl:apply-templates select="type"/>
    End Class
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="enumvalue" mode="Code">
    <xsl:apply-templates select="." mode="Simple"/>
    <xsl:text>
        </xsl:text>
    <xsl:value-of select="@name"/>
    <xsl:if test="@value">=<xsl:apply-templates select="@value"/></xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="element" mode="Code">
    <xsl:variable name="Type">
      <xsl:apply-templates select="@desc"/>
      <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
        <xsl:with-param name="CurrentPackageName">
          <xsl:value-of select="ancestor::class/parent::package/@name"/>
        </xsl:with-param>
      </xsl:apply-templates>
      <xsl:if test="@size"><xsl:value-of select="concat('(',@size,')')"/></xsl:if>
      <xsl:if test="@sizeref">(<xsl:apply-templates select="id(@sizeref)" mode="FullPackageName"/>)</xsl:if>
    </xsl:variable>
    <xsl:variable name="VarName">
      <xsl:apply-templates select="@desc" mode="TypePrefix"/>
      <xsl:apply-templates select="id(@idref)" mode="TypePrefix"/>
      <xsl:value-of select="@name"/>
    </xsl:variable>
    <xsl:apply-templates select="." mode="Simple"/>
        Public <xsl:value-of select="concat($VarName,' As ',$Type)"/>
  </xsl:template>
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
  <xsl:template match="type" mode="Access">
    <xsl:apply-templates select="."/>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="property" mode="Access">
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
	<xsl:variable name="Modifier">
  	  <xsl:choose>
	    <xsl:when test="get/@range='no'">WriteOnly </xsl:when>
	    <xsl:when test="set/@range='no'">ReadOnly </xsl:when>
	  </xsl:choose>
	</xsl:variable>
    <xsl:apply-templates select="comment" mode="Simple"/>
	<xsl:text>
    </xsl:text>
	<xsl:if test="@behaviour='Default'">Default </xsl:if>
	<xsl:value-of select="$Range"/>
	<xsl:if test="@member='class'">Shared </xsl:if>
	<xsl:value-of select="$Modifier"/>
    <xsl:text>Property </xsl:text>
	<xsl:value-of select="concat(@name,'() As ',$Type)"/>
    <xsl:if test="get[@range!='no']">
    Get
        Return <xsl:value-of select="$VarName"/>
    End Get</xsl:if>
    <xsl:if test="set[@range!='no']">
    Set <xsl:value-of select="concat('(ByVal ',$SetParam,' As ',$Type)"/>)
        <xsl:value-of select="concat($VarName,' = ',$SetParam)"/>
    End Set</xsl:if>
    End Property
  </xsl:template>
  <!-- ======================================================================= -->
	<xsl:template match="child | father" mode="Access">
	  <xsl:param name="CurrentPackageName"/>
	  <xsl:param name="CurrentClassID"/>
	  <xsl:param name="Comment"/>
	  <xsl:variable name="Type">
	    <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
		  <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
		</xsl:apply-templates>
      </xsl:variable>
      <xsl:variable name="Accessors">
        <xsl:choose>
          <xsl:when test="get[@range!='no'] and set[@range!='no']"/>
          <xsl:when test="get[@range='no'] and set[@range='no']">None </xsl:when>
          <xsl:when test="get[@range!='no']">ReadOnly </xsl:when>
          <xsl:when test="set[@range!='no']">WriteOnly </xsl:when>
        </xsl:choose>
      </xsl:variable>
      <xsl:if test="$Accessors!='None '">
    ''' &lt;summary&gt;<xsl:value-of select="$Comment"/>&lt;/summary&gt;
    Public <xsl:if test="@member='class'">Shared </xsl:if>
        <xsl:value-of select="$Accessors"/>
        <xsl:value-of select="concat('Property ',@name,'() As ',$Type)"/>
        <xsl:if test="get[@range!='no']">
        Get
            Return <xsl:value-of select="concat($PrefixMember,@name)"/>
        End Get
        </xsl:if>
        <xsl:if test="set[@range!='no']">
        Set <xsl:value-of select="concat('(ByVal ',$SetParam,' As ',$Type)"/>)
            <xsl:value-of select="concat($PrefixMember,@name,' = ',$SetParam)"/>
        End Set</xsl:if>
    End Property
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
       <xsl:if test="get[@range='no'] and set[@range='no']">
    ''' &lt;summary&gt;<xsl:value-of select="$Comment"/>&lt;/summary&gt;
    </xsl:if>
    <xsl:if test="get[@range!='no'] or set[@range!='no']">
      <xsl:text>
    </xsl:text>
    </xsl:if>
    <xsl:apply-templates select="@range" mode="Code"/>
    <xsl:value-of select="concat($PrefixMember,@name)"/> As <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
          <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
        </xsl:apply-templates>
        <xsl:text>
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
  <xsl:template match="list">
    <xsl:param name="Range"/>
    <xsl:param name="ClassName"/>
    <xsl:param name="MemberName"/>
    <xsl:param name="CurrentClassID"/>
    <xsl:param name="Comment"/>
    <xsl:variable name="Index">
      <xsl:apply-templates select="@index-desc"/>
      <xsl:apply-templates select="id(@index-idref)" mode="FullPackageName">
        <xsl:with-param name="CurrentPackage" select="id($CurrentClassID)/parent::package/@name"/>
      </xsl:apply-templates><xsl:if test="@level='1'">*</xsl:if>
    </xsl:variable>
    <xsl:variable name="Container">
      <xsl:choose>
        <xsl:when test="id(@idref)[self::reference]"><xsl:value-of select="id(@idref)/@container"/></xsl:when>
        <xsl:otherwise><xsl:value-of select="count(id(@idref)/model)"/></xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="TypeList">
      <xsl:apply-templates select="@desc"/>
      <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
        <xsl:with-param name="CurrentPackage" select="id($CurrentClassID)/parent::package/@name"/>
      </xsl:apply-templates>
      <xsl:choose>
        <xsl:when test="number($Container)=2">
          <xsl:text>(Of </xsl:text><xsl:value-of select="$Index"/>, <xsl:value-of select="$ClassName"/><xsl:text>)</xsl:text>
        </xsl:when>
        <xsl:when test="number($Container)=1">
          <xsl:text>(Of </xsl:text><xsl:value-of select="$ClassName"/><xsl:text>)</xsl:text>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="number($Container)!=3">
<xsl:text xml:space="preserve">
    </xsl:text>
    <xsl:value-of select="$Range"/>Class <xsl:value-of select="concat('TList',$MemberName)"/>
        Inherits <xsl:value-of select="$TypeList"/>
    End Class

    ''' &lt;summary&gt;<xsl:value-of select="$Comment"/>&lt;/summary&gt;
    <xsl:value-of select="$Range"/><xsl:if test="@member='class'">Shared </xsl:if>
    <xsl:value-of select="concat($PrefixMember,'t',$PrefixList,$MemberName,' As ','TList',$MemberName)"/>
    <xsl:text>
    </xsl:text>
    </xsl:when>
    <xsl:otherwise>
    ''' &lt;summary&gt;<xsl:value-of select="$Comment"/>&lt;/summary&gt;
    <xsl:value-of select="$Range"/><xsl:if test="@member='class'">Shared </xsl:if>
    <xsl:value-of select="concat($PrefixMember,'t',$PrefixList,$MemberName,' As ',$TypeList)"/>
    <xsl:text>
    </xsl:text>
    </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="array">
    <xsl:param name="Range"/>
    <xsl:param name="ClassName"/>
    <xsl:param name="MemberName"/>
    <xsl:param name="CurrentClassID"/>
    <xsl:param name="Comment"/>

    <xsl:variable name="Size">
      <xsl:if test="not(@size) and not(@sizeref)">0</xsl:if>
      <xsl:value-of select="@size"/>
      <xsl:apply-templates select="id(@sizeref)" mode="FullPackageName">
        <xsl:with-param name="CurrentClassName" select="id($CurrentClassID)/@name"/>
        <xsl:with-param name="CurrentPackage" select="id($CurrentClassID)/parent::package/@name"/>
      </xsl:apply-templates>
    </xsl:variable>
    ''' &lt;summary&gt;<xsl:value-of select="$Comment"/>&lt;/summary&gt;
    <xsl:value-of select="concat($Range,$PrefixMember,'array',$MemberName,'(',$Size,')',' As ',$ClassName)"/>
    <xsl:text>
    </xsl:text>
  </xsl:template>
<!-- ======================================================================= -->
</xsl:stylesheet>


















