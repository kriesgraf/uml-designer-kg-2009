<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
<!-- ======================================================================= -->
  <xsl:output method="xml" cdata-section-elements="code" encoding="ISO-8859-1"/>
<!-- ======================================================================= -->
  <xsl:param name="UmlFolder">E:\Documents\Mes projets\UML_project</xsl:param>
  <xsl:param name="InputClass">class18</xsl:param>
  <xsl:param name="InputPackage"/>
  <!-- ======================================================================= -->
  <xsl:variable name="FileLanguage"><xsl:value-of select="$UmlFolder"/>\language.xml</xsl:variable>
  <xsl:variable name="GetName" select="document($FileLanguage)//GetName/text()"/>
  <xsl:variable name="SetName" select="document($FileLanguage)//SetName/text()"/>
  <xsl:variable name="PrefixList" select="document($FileLanguage)//PrefixList/text()"/>
  <xsl:variable name="SuffixIterator" select="document($FileLanguage)//SuffixIterator/text()"/>
  <xsl:variable name="PrefixMember" select="document($FileLanguage)//PrefixMember/text()"/>
  <xsl:variable name="PrefixArray" select="document($FileLanguage)//PrefixArray/text()"/>
  <xsl:variable name="TypeAccess" select="document($FileLanguage)//TypeAccess/text()"/>
  <xsl:variable name="SetParam" select="document($FileLanguage)//SetParam/text()"/>
<!-- ======================================================================= -->
  <xsl:include href="cpp-types.xsl"/>
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
        <xsl:comment>InputClass=<xsl:value-of select="$InputClass"/></xsl:comment>
        <xsl:apply-templates select="//root/package[descendant::class[@id=$InputClass]]" mode="Code"/>
        <xsl:apply-templates select="//root/class[@id=$InputClass]" mode="Code"/>
      </xsl:when>
      <xsl:when test="$InputPackage!=''">
        <xsl:comment>InputPackage=<xsl:value-of select="$InputPackage"/></xsl:comment>
        <xsl:apply-templates select="//root/package[@id=$InputPackage or descendant::package[@id=$InputPackage]]" mode="Code"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:comment>Otherwise</xsl:comment>
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
      <xsl:attribute name="name"><xsl:value-of select="@name"/>.h</xsl:attribute>
#ifndef <xsl:apply-templates select="." mode="Entete"/>
#define <xsl:apply-templates select="." mode="Entete"/>

    <xsl:variable name="Request1"><xsl:call-template name="FullIncludeName"/></xsl:variable>
    <xsl:variable name="Request3"><xsl:call-template name="FullReferenceName"/></xsl:variable>
    <!--
    <INC><xsl:copy-of select="$Request1"/></INC>
    <REF><xsl:copy-of select="$Request3"/></REF>
     -->
    <xsl:variable name="Request2">
    <xsl:for-each select="msxsl:node-set($Request3)/reference">
      <xsl:if test="not(current()/@name=msxsl:node-set($Request1)/reference/@name)">
        <xsl:copy-of select="."/>
      </xsl:if>
    </xsl:for-each>
    </xsl:variable>
    <xsl:variable name="CurrentPackage"><xsl:value-of select="parent::package/@name"/></xsl:variable>
    <xsl:text xml:space="preserve">
</xsl:text>

    <xsl:for-each select="msxsl:node-set($Request1)/reference[generate-id()=generate-id(key('include',@value)[1])]">
#include <xsl:value-of select="@value"/></xsl:for-each>
<xsl:text xml:space="preserve">
</xsl:text>
    <xsl:for-each select="msxsl:node-set($Request2)/reference[@package='' and generate-id()=generate-id(key('class',@name)[1])]">
class <xsl:value-of select="@name"/>;</xsl:for-each>
<xsl:text xml:space="preserve">
</xsl:text>
    <xsl:for-each select="msxsl:node-set($Request2)/reference/@package[.!= '' and .!=$CurrentPackage and generate-id()=generate-id(key('package',.)[1])]">
		<xsl:sort select="."/>
		
namespace <xsl:value-of select="."/>
{   <xsl:for-each select="//reference[@package=current() and generate-id()=generate-id(key('class',@name)[1])]">
    class <xsl:value-of select="@name"/>;</xsl:for-each>
}	</xsl:for-each>
    <xsl:if test="parent::package">
    <xsl:text xml:space="preserve">
</xsl:text>
    <xsl:apply-templates select="import/body" mode="Declaration"/>
    <xsl:apply-templates select="parent::package/comment"/>

namespace <xsl:value-of select="parent::package/@name"/>
{
    </xsl:if>
   <xsl:for-each select="msxsl:node-set($Request2)/reference[@package!= '' and @package=$CurrentPackage and generate-id()=generate-id(key('class',@name)[1])]">
    class <xsl:value-of select="@name"/>;
    </xsl:for-each>
    <xsl:apply-templates select="comment"/>
    <xsl:apply-templates select="model" mode="Class"><xsl:sort select="@id"/></xsl:apply-templates>
    class <xsl:value-of select="@name"/><xsl:call-template name="Implements"/>
    {<xsl:if test="property[@member='class' and type/@modifier='const']"><xsl:call-template name="Constants"/></xsl:if>
    <xsl:if test="typedef"><xsl:call-template name="Typedef"/></xsl:if>
  <xsl:if test="property"><xsl:call-template name="Properties"/></xsl:if>
  <xsl:variable name="ClassID" select="@id"/>
  <xsl:if test="id(collaboration/@idref)[(child/@idref=$ClassID and father/@range!='no') or father/@idref=$ClassID]">
    // =========================================================================
    // Relationships
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
  </xsl:if>
    // =========================================================================
    //#Region "ConstructorsDestructor/Methods"
  <xsl:call-template name="Functions">
    <xsl:with-param name="Range">private</xsl:with-param>
  </xsl:call-template>
  <xsl:call-template name="Functions">
   <xsl:with-param name="Range">protected</xsl:with-param>
  </xsl:call-template>
  <xsl:call-template name="Functions">
    <xsl:with-param name="Range">public</xsl:with-param>
  </xsl:call-template>
    //#End Region
    
    // =========================================================================
    //#Region "Other declarations (Not managed)"
    //#End Region
    };
    <xsl:if test="parent::package">
}
    </xsl:if>
#endif
    </xsl:element>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Typedef">

    // =========================================================================
    //#Region "Predefined types"<xsl:if test="typedef[variable/@range='public']">

    public:<xsl:apply-templates select="typedef[variable/@range='public']" mode="Code"/>
    </xsl:if>
    <xsl:if test="typedef[variable/@range='protected']">

    protected:<xsl:apply-templates select="typedef[variable/@range='protected']" mode="Code"/>
    </xsl:if>
    <xsl:if test="typedef[variable/@range='private']">

    private:<xsl:apply-templates select="typedef[variable/@range='private']" mode="Code"/>
    </xsl:if>
    //#End Region
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Implements">
    <xsl:if test="inherited"> : </xsl:if>
    <xsl:apply-templates select="inherited" mode="Code"/>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="model" mode="Class">
    <xsl:if test="position()=1">
    template &lt;</xsl:if>
    <xsl:value-of select="concat('class ',@name)"/>
    <xsl:if test="position()!=last()">, </xsl:if>
    <xsl:if test="position()=last()">&gt;</xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="inherited" mode="Code">
    <xsl:value-of select="concat(@range,' ')"/>
    <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
      <xsl:with-param name="CurrentPackageName" select="parent::class/parent::package/@name"/>
    </xsl:apply-templates>
    <xsl:if test="position()=1 and last() &gt;1">, </xsl:if>
    <xsl:if test="position()!=1 and position()!=last()">, </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Constants">
    // =========================================================================
    //#Region "Constants"<xsl:if test="property[variable/@range='public' and @member='class' and type/@modifier='const']">

    public:<xsl:apply-templates select="property[variable/@range='public' and @member='class' and type/@modifier='const']" mode="Code"><xsl:sort select="@name"/></xsl:apply-templates>
    </xsl:if>
    <xsl:if test="property[variable/@range='protected' and @member='class' and type/@modifier='const']">

    protected:<xsl:apply-templates select="property[variable/@range='protected' and @member='class' and type/@modifier='const']" mode="Code"><xsl:sort select="@name"/></xsl:apply-templates>
    </xsl:if>
    <xsl:if test="property[variable/@range='private' and @member='class' and type/@modifier='const']">

    private:<xsl:apply-templates select="property[variable/@range='private' and @member='class' and type/@modifier='const']" mode="Code"><xsl:sort select="@name"/></xsl:apply-templates>
    </xsl:if>
    //#End Region
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Properties">
    <xsl:if test="property[@attribute='yes']">
    // =========================================================================
    //#Region "Member variables"
        <xsl:call-template name="Attributes">
        <xsl:with-param name="Range">public</xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="Attributes">
        <xsl:with-param name="Range">protected</xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="Attributes">
        <xsl:with-param name="Range">private</xsl:with-param>
        </xsl:call-template>
    //#End Region
    </xsl:if>
	<xsl:if test="property[variable/@range!='public'][get[@range!='no'] or set[@range!='no']]">
    // =========================================================================
    //#Region "Properties"
        <xsl:call-template name="Accessors">
        <xsl:with-param name="Range">public</xsl:with-param>
        </xsl:call-template>
        <xsl:call-template name="Accessors">
        <xsl:with-param name="Range">protected</xsl:with-param>
        </xsl:call-template>
    //#End Region
    </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Attributes">
    <xsl:param name="Range"/>
    <xsl:if test="property[variable/@range=$Range and (@member!='class' or type/@modifier!='const')]">
<xsl:text xml:space="preserve">
    </xsl:text><xsl:value-of select="$Range"/>:<xsl:apply-templates select="property[variable/@range=$Range and (@member!='class' or type/@modifier!='const')]" mode="Code"><xsl:sort select="@name"/></xsl:apply-templates>
    </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Accessors">
    <xsl:param name="Range"/>
    <xsl:if test="property[get[@range=$Range] or set[@range=$Range]]">
	<xsl:text xml:space="preserve">
    </xsl:text><xsl:value-of select="$Range"/>:
      <xsl:apply-templates select="property[variable/@range!='public'][get[@range!='no'] or set[@range!='no']]" mode="Access">
        <xsl:with-param name="Range" select="$Range"/>
        <xsl:sort select="@name"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template name="Relationships">
    <xsl:param name="Range"/>
    <xsl:param name="ClassID"/>
    <xsl:variable name="CurrentPackageName" select="id($ClassID)/parent::package/@name"/>
    <xsl:if test="//relationship[($Range= child/@range and father/@idref=$ClassID) or ($Range= father/@range and child/@idref=$ClassID) or               (($Range= child/get/@range or $Range= child/set/@range) and father/@idref=$ClassID) or (($Range= father/get/@range or $Range= father/set/@range) and child/@idref=$ClassID)]">
<xsl:text xml:space="preserve">
    </xsl:text><xsl:value-of select="$Range"/><xsl:text>:</xsl:text>
		<xsl:apply-templates select="//relationship/child[$Range= @range and preceding-sibling::father/@idref=$ClassID]" mode="Relation">
			<xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
			<xsl:with-param name="CurrentClassID" select="$ClassID"/>
		</xsl:apply-templates>
		<xsl:apply-templates select="//relationship/father[$Range= @range and following-sibling::child/@idref=$ClassID]" mode="Relation">
			<xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
			<xsl:with-param name="CurrentClassID" select="$ClassID"/>
		</xsl:apply-templates>
		<xsl:apply-templates select="//relationship/child[($Range= get/@range or $Range= set/@range) and preceding-sibling::father/@idref=$ClassID]" mode="Access">
			<xsl:with-param name="Range" select="$Range"/>
			<xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
			<xsl:with-param name="CurrentClassID" select="$ClassID"/>
		</xsl:apply-templates>
		<xsl:apply-templates select="//relationship/father[($Range= get/@range or $Range= set/@range) and following-sibling::child/@idref=$ClassID]" mode="Access">
			<xsl:with-param name="Range" select="$Range"/>
			<xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
			<xsl:with-param name="CurrentClassID" select="$ClassID"/>
		</xsl:apply-templates>
	</xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="@inline" mode="Constructor">  <!-- Manage constructor inline or not ! -->
     <xsl:param name="ClassName"/><xsl:if test=".='both' or .='constructor'">inline </xsl:if>
     <xsl:value-of select="$ClassName"/>()<xsl:if test=".='both' or .='constructor'">
		 <xsl:apply-templates select="parent::class/property[variable/@value or variable/@valref][@member='object' and type/@modifier='var']" mode="InlineMembers"/>
    {<xsl:apply-templates select="parent::class/inline/body[@type='constructor']"/>
    };
     </xsl:if>
     <xsl:if test=".='none' or .='destructor'">;
     </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="@inline" mode="Destructor">  <!-- Manage destructor inline or not ! -->
     <xsl:param name="ClassName"/>
     <xsl:if test=".='both' or .='destructor'">inline </xsl:if>virtual ~<xsl:value-of select="$ClassName"/>()<xsl:if test=".='both' or .='destructor'">
    {<xsl:apply-templates select="parent::class/inline/body[@type='destructor']"/>
    };
     </xsl:if>
     <xsl:if test=".='none' or .='constructor'">;
     </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="@constructor">
    /// Default constructor
    <xsl:apply-templates select="parent::class/@inline" mode="Constructor">
      <xsl:with-param name="ClassName" select="parent::class/@name"/>
    </xsl:apply-templates>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="@destructor">
    /// Destructor
    <xsl:apply-templates select="parent::class/@inline" mode="Destructor">
      <xsl:with-param name="ClassName" select="parent::class/@name"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Functions">
    <xsl:param name="Range"/>
    <xsl:if test="@constructor=$Range or @destructor=$Range or method[return/variable/@range=$Range or @constructor=$Range]">
    <xsl:text xml:space="preserve">
    </xsl:text>
    <xsl:value-of select="$Range"/>:
      <xsl:apply-templates select="@constructor[.=$Range]"/>
      <xsl:apply-templates select="@destructor[.=$Range]"/>
      <xsl:apply-templates select="method[return/variable/@range=$Range or @constructor=$Range]">
        <xsl:sort select="@name"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="typedef">
    <xsl:apply-templates select="comment"/>
    <xsl:variable name="Type">
      <xsl:apply-templates select="type"/>
      <xsl:text> </xsl:text>
      <xsl:apply-templates select="variable" mode="Typedef">
        <xsl:with-param name="Name" select="@name"/>
      </xsl:apply-templates>
    </xsl:variable>
    typedef <xsl:value-of select="$Type"/>;
    <xsl:if test="type/list/@iterator='yes'">
    <xsl:value-of select="concat('typedef ',@name,'::iterator ',@name,'Iterator')"/>;
    </xsl:if>
    <xsl:if test="type/array">
    <xsl:value-of select="concat('typedef ',@name,'* ',@name,'Iterator')"/>;
    </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="enumvalue">
    <xsl:if test="position()=1">
    {
        </xsl:if>
    <xsl:value-of select="@name"/>
    <xsl:if test="@value">=<xsl:apply-templates select="@value"/></xsl:if>
    <xsl:if test="position()&lt;last()">, </xsl:if>
    <xsl:value-of select="concat(' ///&lt;',.)"/><xsl:text xml:space="preserve">
        </xsl:text>
    <xsl:if test="position()=last()">
    }</xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="element">
    <xsl:variable name="Type">
      <xsl:if test="@modifier='const'">const </xsl:if>
      <xsl:apply-templates select="@desc"/>
      <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
        <xsl:with-param name="CurrentPackageName">
          <xsl:value-of select="ancestor::class/parent::package/@name"/>
        </xsl:with-param>
      </xsl:apply-templates>
      <xsl:if test="@level='1'">*</xsl:if>
      <xsl:if test="@level='2'">**</xsl:if>
    </xsl:variable>
    <xsl:variable name="VarName">
      <xsl:apply-templates select="@desc" mode="TypePrefix"/>
      <xsl:apply-templates select="id(@idref)" mode="TypePrefix"/>
      <xsl:value-of select="@name"/>
      <xsl:if test="@size"><xsl:value-of select="concat('[',@size,']')"/></xsl:if>
      <xsl:if test="@sizeref">[<xsl:apply-templates select="id(@sizeref)" mode="FullPackageName"/>]</xsl:if>
    </xsl:variable>
    <xsl:if test="position()=1">
    {
        </xsl:if>
    <xsl:value-of select="concat($Type,' ',$VarName)"/>;    <xsl:value-of select="concat(' ///&lt;',.)"/><xsl:text xml:space="preserve">
        </xsl:text>
    <xsl:if test="position()=last()">
    }</xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="method">
    <xsl:apply-templates select="comment" mode="Method"/>
    <xsl:apply-templates select="param" mode="Comment"/>
    <xsl:apply-templates select="return" mode="Comment"/>
    */
    <xsl:call-template name="Method"/>;
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="type" mode="Access">
	  <xsl:choose>
		  <xsl:when test="contains(@desc,'ostringstream')">std::string</xsl:when>
		  <xsl:when test="contains(@desc,'bool')">bool</xsl:when>
		  <xsl:when test="contains(@desc,'Bool')">bool</xsl:when>
		  <xsl:when test="contains(id(@idref)/@name,'Bool')">bool</xsl:when>
		  <xsl:when test="contains(id(@idref)/@name,'Bool')">bool</xsl:when>
			<xsl:otherwise>
    		<xsl:apply-templates select="."/>
			</xsl:otherwise>
		</xsl:choose>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="property" mode="Access">
    <xsl:param name="Range"/>
    
    <xsl:variable name="ClassImpl"><xsl:value-of select="parent::class/@implementation"/></xsl:variable>
  
	<xsl:variable name="VarName">
	    <xsl:value-of select="$PrefixMember"/>
	      <xsl:apply-templates select="type" mode="TypePrefix"/>
	      <xsl:apply-templates select="variable" mode="TypePrefix"/>
	    <xsl:value-of select="@name"/>
	</xsl:variable>

	<xsl:variable name="Type">
		<xsl:choose>
			<xsl:when test="@access-value!='none'">
				<xsl:call-template name="PredefinedType">
					<xsl:with-param name="Type" select="@access-value"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="type/enumvalue">
				<xsl:value-of select="concat('enum  E',@name)"/>
			</xsl:when>
			<xsl:when test="type/element">
				<xsl:value-of select="concat('struct  T',@name)"/>
			</xsl:when>
			<xsl:otherwise>
	      		<xsl:apply-templates select="type" mode="Access"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

    <xsl:variable name="Const"><xsl:if test="set/@by='ref'">const </xsl:if></xsl:variable>
    <xsl:variable name="Get"><xsl:if test="get/@by='ref'">&amp; </xsl:if></xsl:variable>
    <xsl:variable name="Set"><xsl:if test="set/@by='ref'">&amp; </xsl:if></xsl:variable>

	<xsl:variable name="SetSteatment">
		<xsl:choose>
			<xsl:when test="contains(type/@desc,'ostringstream')">
				<xsl:value-of select="concat($VarName,' &lt;&lt; ',$SetParam)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat($VarName,' = ',$SetParam)"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:if test="get[@range=$Range]">
	<xsl:text xml:space="preserve">
    </xsl:text>
	<xsl:if test="@overridable='yes'">virtual </xsl:if>
	<xsl:if test="@member='class'">static </xsl:if>
    <xsl:if test="get/@modifier='const'">const </xsl:if>
    <xsl:value-of select="concat($Type,' ',$Get,$GetName,@name)"/>() <xsl:if test="@member!='class'">const</xsl:if><xsl:if test="$ClassImpl='abstract'">= 0;
    </xsl:if>
    <xsl:if test="$ClassImpl!='abstract'">
    {<xsl:if test="@attribute='yes'">
        return <xsl:value-of select="$VarName"/><xsl:if test="contains(type/@desc,'ostringstream')">.str()</xsl:if>;</xsl:if>
    }
    </xsl:if>
    </xsl:if>
    <xsl:if test="set[@range=$Range]">
    <xsl:text xml:space="preserve">
    </xsl:text>
	<xsl:if test="@overridable='yes'">virtual </xsl:if>
    <xsl:if test="@member='class'">static </xsl:if>void <xsl:value-of select="concat($SetName,@name,'(',$Const,$Type,' ',$Set,$SetParam)"/>)<xsl:if test="$ClassImpl='abstract'">= 0;
    </xsl:if>
    <xsl:if test="$ClassImpl!='abstract'">
    {<xsl:if test="@attribute='yes'">
        <xsl:text xml:space="preserve">
        </xsl:text>
        <xsl:value-of select="$SetSteatment"/>;</xsl:if>
    }
    </xsl:if>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
	<xsl:template match="child | father" mode="Access">
		<xsl:param name="Range"/>
		<xsl:param name="CurrentPackageName"/>
		<xsl:param name="CurrentClassID"/>
		<xsl:variable name="Type">
			<xsl:apply-templates select="id(@idref)" mode="FullPackageName">
				<xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
			</xsl:apply-templates>
			<xsl:if test="@level='1'">*</xsl:if>
		</xsl:variable>
		<xsl:variable name="Level2"><xsl:if test="@level='1'">p</xsl:if></xsl:variable>
	<xsl:if test="get[@range=$Range]">
	<xsl:text xml:space="preserve">
    </xsl:text>
	<xsl:if test="@member='class'">static </xsl:if>
    <xsl:value-of select="concat($Type,' ',$GetName,@name)"/>()
    {
        return <xsl:value-of select="concat($PrefixMember,$Level2,@name)"/>;
    }
    
    </xsl:if>
    <xsl:if test="set[@range=$Range]"><xsl:text xml:space="preserve">
    </xsl:text>
	<xsl:if test="@member='class'">static </xsl:if>void <xsl:value-of select="concat($SetName,@name,'(',$Type,' ',$Level2,@name)"/>)
    {
        <xsl:value-of select="concat($PrefixMember,$Level2,@name)"/>= <xsl:value-of select="concat($Level2,@name)"/>;
    }

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
    //! <xsl:value-of select="$Comment"/>.
    <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
          <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
        </xsl:apply-templates>
        <xsl:variable name="Level1"><xsl:if test="@level='1'">*</xsl:if></xsl:variable>
        <xsl:variable name="Level2"><xsl:if test="@level='1'">p</xsl:if></xsl:variable>
        <xsl:value-of select="concat($Level1,' ',$PrefixMember,$Level2,@name)"/>;
     </xsl:when>
     <xsl:otherwise>
        <xsl:apply-templates select="*">
          <xsl:with-param name="ClassName">
            <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
              <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
            </xsl:apply-templates>
            <xsl:if test="@level='1'">*</xsl:if>
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
    <xsl:variable name="TypeList">
      <xsl:apply-templates select="@desc"/>
      <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
        <xsl:with-param name="CurrentPackage" select="id($CurrentClassID)/parent::package/@name"/>
      </xsl:apply-templates>
      <xsl:if test="@type='indexed'"><xsl:value-of select="concat('&lt;',$Index,', ')"/></xsl:if>
      <xsl:if test="@type='simple'">&lt;</xsl:if>
      <xsl:value-of select="$ClassName"/>
      <!--xsl:if test="@type='indexed'"><xsl:value-of select="concat(', std::less&lt;',$Index,'&gt; ')"/></xsl:if-->
      <xsl:text>&gt;</xsl:text>
    </xsl:variable>
<xsl:text xml:space="preserve">
    </xsl:text>
    <xsl:value-of select="concat('typedef ',$TypeList,' TList',$MemberName)"/>;
    <xsl:if test="@iterator='yes'"><xsl:value-of select="concat('typedef TList',$MemberName,'::iterator T',$MemberName,$SuffixIterator)"/>;</xsl:if><xsl:text xml:space="preserve">
    </xsl:text>
    //! <xsl:value-of select="$Comment"/>.
    <xsl:if test="@member='class'">static </xsl:if>
    <xsl:value-of select="concat('TList',$MemberName,' ',$PrefixMember,'t',$PrefixList,$MemberName)"/>;
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="array">
    <xsl:param name="ClassName"/>
    <xsl:param name="MemberName"/>
    <xsl:param name="CurrentClassID"/>
    <xsl:variable name="Size">
      <xsl:if test="not(@size) and not(@sizeref)">0</xsl:if>
      <xsl:value-of select="@size"/>
      <xsl:apply-templates select="@sizeref" mode="FullPackageName">
        <xsl:with-param name="CurrentClassName" select="id($CurrentClassID)/@name"/>
        <xsl:with-param name="CurrentPackage" select="id($CurrentClassID)/parent::package/@name"/>
      </xsl:apply-templates>
    </xsl:variable>
    <xsl:value-of select="concat($ClassName,' ',$PrefixMember,'array',$MemberName,'[',$Size,']')"/>;
  </xsl:template>
<!-- ======================================================================= -->
<xsl:template match="class | package" mode="Entete">
  <xsl:if test="parent::package"><xsl:apply-templates select="parent::package" mode="Entete"/>_</xsl:if>
  <xsl:if test="parent::root">UNIQUE_</xsl:if>
  <xsl:value-of select="translate(@name,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
</xsl:template>
<!-- ======================================================================= -->
</xsl:stylesheet>


<!-- Stylus Studio meta-information - (c) 2004-2007. Progress Software Corporation. All rights reserved.
<metaInformation>
<scenarios ><scenario default="yes" name="Test" userelativepaths="yes" externalpreview="no" url="ListOfTypes.xprj" htmlbaseurl="" outputurl="" processortype="msxmldotnet" useresolver="no" profilemode="0" profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal" customvalidator="" ><parameterValue name="InputClass" value="'class18'"/><parameterValue name="Language" value="'LanguageCplusPlus.xml'"/><parameterValue name="UmlFolder" value="'E:\Documents\Mes projets\UML_project'"/><advancedProp name="sInitialMode" value=""/><advancedProp name="bXsltOneIsOkay" value="true"/><advancedProp name="bSchemaAware" value="false"/><advancedProp name="bXml11" value="false"/><advancedProp name="iValidation" value="0"/><advancedProp name="bExtensions" value="true"/><advancedProp name="iWhitespace" value="0"/><advancedProp name="sInitialTemplate" value=""/>

<advancedProp name="bTinyTree" value="true"/><advancedProp name="bWarnings" value="true"/><advancedProp name="bUseDTD" value="false"/><advancedProp name="iErrorHandling" value="fatal"/></scenario></scenarios><MapperMetaTag><MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/><MapperBlockPosition></MapperBlockPosition><TemplateContext></TemplateContext><MapperFilter side="source"></MapperFilter></MapperMetaTag>
</metaInformation>
-->

