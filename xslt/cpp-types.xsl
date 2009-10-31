<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ======================================================================= -->
  <xsl:param name="Language"><xsl:value-of select="$LanguageFolder"/>\LanguageCplusPlus.xml</xsl:param>
  <!-- ======================================================================= -->
  <xsl:template match="@desc | @index | @index-desc">
    <xsl:call-template name="PredefinedType">
       <xsl:with-param name="Type" select="."/>
    </xsl:call-template>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="PredefinedType">
    <xsl:param name="Type"/>
    <xsl:variable name="FinalType" select="document($Language)//type[@name=$Type]/@implementation"/>
    <xsl:value-of select="$FinalType"/>
    <xsl:if test="string-length($FinalType)=0">
      <xsl:value-of select="$Type"/>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type" mode="TypePrefix">
    <!--xsl:for-each select="@*">{<xsl:value-of select="name()"/>}</xsl:for-each-->
    <xsl:apply-templates select="@level" mode="TypePrefix"/>
    <xsl:apply-templates select="@desc" mode="TypePrefix"/>
    <xsl:apply-templates select="id(@idref)" mode="TypePrefix"/>
    <xsl:apply-templates select="list | enumvalue[1] | element" mode="TypePrefix"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference" mode="TypePrefix">
    <xsl:choose>
      <xsl:when test="contains(translate(@name,'B','b'),'bool')">b</xsl:when>
      <xsl:when test="contains(translate(@name,'I','i'),'int')">i</xsl:when>
      <xsl:when test="@type='typedef'">t</xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class | interface" mode="TypePrefix"/>
  <!-- ======================================================================= -->
  <xsl:template match="typedef" mode="TypePrefix">
    <xsl:choose>
      <xsl:when test="descendant::enumvalue">e</xsl:when>
      <xsl:otherwise>t</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="element" mode="TypePrefix"><xsl:if test="position()=1"><xsl:value-of select="parent::type/@struct"/></xsl:if></xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="enumvalue" mode="TypePrefix">e</xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="variable" mode="TypePrefix">
    <xsl:apply-templates select="@level" mode="TypePrefix"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@desc | @index | @level | enumvalue" mode="TypePrefix">
    <xsl:variable name="ArrayOk">
      <xsl:if test="parent::*/following-sibling::variable/@size or parent::*/following-sibling::variable/@sizeref">Ok</xsl:if>
      <xsl:if test="parent::*/@size or parent::*/@sizeref">Ok</xsl:if>
      </xsl:variable>
    <xsl:variable name="PointerOk">
      <xsl:if test="parent::*//@level!='0'">Ok</xsl:if>
      </xsl:variable>
    <!--xsl:value-of select="concat('[',name(),'=',.,'PointerOk=',$PointerOk,'ArrayOk=',$ArrayOk,']')"/-->
    <xsl:choose>
      <xsl:when test=".='0'"/>
      <xsl:when test=".='1'">p</xsl:when>
      <xsl:when test=".='2'">h</xsl:when>
      <xsl:when test=".='bool'">b</xsl:when>
      <xsl:when test=".='schar' and $PointerOk='Ok'">sz</xsl:when>
      <xsl:when test=".='schar' and $ArrayOk='Ok'">sz</xsl:when>
      <xsl:when test=".='char' and $PointerOk='Ok'">sz</xsl:when>
      <xsl:when test=".='char' and $ArrayOk='Ok'">sz</xsl:when>
      <xsl:when test="name()='enumvalue'">e</xsl:when>
      <xsl:otherwise>
          <xsl:value-of select="document($Language)//type[@name=current()]/@prefix"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="class" mode="InlineMembers">
    <xsl:apply-templates select="inherited | property[variable/@value or variable/@valref]" mode="InlineMembers"/>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="inherited" mode="InlineMembers">
    <xsl:variable name="Params">
      <xsl:for-each select="id(@idref)/method[@constructor!='no'][1]/param">
        <xsl:value-of select="@name"/><xsl:if test="position()!=last()">, </xsl:if>
      </xsl:for-each>
    </xsl:variable>
	<xsl:if test="position()=1">
	:    </xsl:if>
	<xsl:if test="position()!=1">
	,    </xsl:if><xsl:value-of select="concat(id(@idref)/@name,'(',$Params,')')"/>
    </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="property" mode="InlineMembers">
	<xsl:variable name="Name">
    <xsl:value-of select="$PrefixMember"/>
    <xsl:apply-templates select="type" mode="TypePrefix"/>
    <xsl:apply-templates select="variable" mode="TypePrefix"/>
    <xsl:value-of select="@name"/>
	</xsl:variable>
	<xsl:variable name="Value">
		<xsl:value-of select="variable/@value"/>
    <xsl:apply-templates select="id(variable/@valref)" mode="FullPackageName">
      <xsl:with-param name="CurrentClassName" select="parent::class/@name"/>
      <xsl:with-param name="CurrentPackageName" select="parent::class/parent::package/@name"/>
		</xsl:apply-templates>
	</xsl:variable>
	<xsl:if test="position()=1">
	:    </xsl:if>
	<xsl:if test="position()!=1">
	,    </xsl:if><xsl:value-of select="concat($Name,'(',$Value,')')"/>
	</xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="property" mode="Code">
    <xsl:param name="BodyOk"/>
    <xsl:if test="$BodyOk!='ok'">
      <xsl:apply-templates select="comment" mode="Details"/>
    </xsl:if>
    <xsl:text xml:space="preserve">
    </xsl:text>
    <xsl:variable name="ConstOk">
      <xsl:if test="@member='class' and type/@modifier='const'">ok</xsl:if>
    </xsl:variable>
    <xsl:if test="$BodyOk!='ok' and @member='class'">static </xsl:if>
    <xsl:variable name="Type"><xsl:apply-templates select="type"/></xsl:variable>
    <xsl:if test="string-length($Type)!=0"><xsl:value-of select="concat($Type,' ')"/></xsl:if>
    <xsl:apply-templates select="variable" mode="Typedef">
      <xsl:with-param name="Name">
        <xsl:if test="$ConstOk!='ok'">
          <xsl:value-of select="$PrefixMember"/>
          <xsl:apply-templates select="type" mode="TypePrefix"/>
          <xsl:apply-templates select="variable" mode="TypePrefix"/>
        </xsl:if>
        <xsl:value-of select="@name"/>
      </xsl:with-param>
    </xsl:apply-templates>
    <xsl:if test="($BodyOk='ok' or $ConstOk='ok') and variable[string-length(@value)!=0 or @valref]">
      <xsl:text>= </xsl:text>
      <xsl:value-of select="variable/@value"/>
      <xsl:apply-templates select="id(variable/@valref)" mode="FullPackageName">
        <xsl:with-param name="CurrentClassName" select="ancestor::class/@name"/>
        <xsl:with-param name="CurrentPackageName" select="ancestor::class/parent::package/@name"/>
      </xsl:apply-templates>
    </xsl:if>;
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="variable" mode="Typedef">
    <xsl:param name="Name"/>
    <xsl:if test="@modifier='const'">
      <xsl:value-of select="concat(@modifier,' ')"/>
    </xsl:if>
    <xsl:if test="@level='1'">*</xsl:if>
    <xsl:if test="@level='2'">**</xsl:if>
    <xsl:value-of select="concat(' ',$Name)"/>
    <xsl:if test="@sizeref or @size">
      <xsl:text>[</xsl:text>
      <xsl:value-of select="@size"/>
      <xsl:apply-templates select="id(@sizeref)" mode="FullPackageName">
        <xsl:with-param name="CurrentClassName" select="ancestor::class/@name"/>
        <xsl:with-param name="CurrentPackageName" select="ancestor::class/parent::package/@name"/>
      </xsl:apply-templates>
      <xsl:text>]</xsl:text>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type">
    <xsl:if test="@modifier='const'">const </xsl:if>
    <xsl:choose>
      <xsl:when test="enumvalue">enum<xsl:if test="parent::property"><xsl:value-of select="concat(' ', $PrefixEnumProperty, parent::property/@name)"/></xsl:if><xsl:apply-templates select="enumvalue"/></xsl:when>
      <xsl:when test="element"><xsl:value-of select="@struct"/><xsl:if test="parent::property"><xsl:value-of select="concat(' ', $PrefixStructProperty, parent::property/@name)"/></xsl:if><xsl:apply-templates select="element"/></xsl:when>
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
              <xsl:with-param name="CurrentPackageName" select="ancestor::class/parent::package/@name"/>
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
          <xsl:with-param name="CurrentPackageName" select="ancestor::class/parent::package/@name"/>
        </xsl:apply-templates>
      </xsl:otherwise>
    </xsl:choose>
		<xsl:if test="not(@struct)">
      <xsl:if test="@level='1'">* </xsl:if>
      <xsl:if test="@level='2'">** </xsl:if>
      <xsl:if test="@by='ref'">&amp; </xsl:if>
		</xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="list" mode="Typedef">
    <xsl:param name="Type"/>
    <xsl:variable name="Index">
      <xsl:apply-templates select="@index-desc"/>
      <xsl:apply-templates select="id(@index-idref)" mode="FullPackageName">
        <xsl:with-param name="CurrentPackage" select="ancestor::class/parent::package/@name"/>
      </xsl:apply-templates>
			<xsl:if test="@level='1'">*</xsl:if>
			<xsl:if test="@level='2'">**</xsl:if>
    </xsl:variable>
		<xsl:value-of select="@desc"/>
    <xsl:apply-templates select="id(@idref)" mode="FullPackageName">
      <xsl:with-param name="CurrentPackage" select="ancestor::class/parent::package/@name"/>
    </xsl:apply-templates>
    <xsl:if test="@type='indexed'"><xsl:value-of select="concat('&lt;',$Index,', ')"/></xsl:if>
    <xsl:if test="@type='simple'">&lt;</xsl:if>
    <xsl:value-of select="$Type"/>
		<xsl:if test="parent::type/@level='1'">*</xsl:if>
		<xsl:if test="parent::type/@level='2'">**</xsl:if>
    <!--xsl:if test="@type='indexed'"><xsl:value-of select="concat(', std::less&lt;',$Index,'&gt; ')"/></xsl:if-->
    <xsl:text>&gt;</xsl:text>
   </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="array" mode="Typedef">
    <xsl:param name="Type"/>
    <xsl:value-of select="$Type"/>
    <xsl:if test="@size">[<xsl:value-of select="@size"/>]</xsl:if>
    <xsl:if test="@sizeref">[<xsl:apply-templates select="id(@sizeref)" mode="FullPackageName"/>]</xsl:if>
</xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="param">
    <xsl:param name="BodyOk"/>
    <xsl:param name="Range"/>
    <xsl:apply-templates select="type"/>
    <xsl:text/>
    <xsl:apply-templates select="variable" mode="Typedef">
      <xsl:with-param name="Name" select="@name"/>
    </xsl:apply-templates>
    <xsl:if test="$BodyOk!='ok' and variable[string-length(@value)!=0 or @valref]">
      <xsl:value-of select="concat('= ',variable/@value)"/>
      <xsl:apply-templates select="id(variable/@valref)" mode="FullPackageName">
        <xsl:with-param name="CurrentClassName" select="ancestor::class/@name"/>
        <xsl:with-param name="CurrentPackageName" select="ancestor::class/parent::package/@name"/>
      </xsl:apply-templates>
    </xsl:if>
    <xsl:if test="position()!=last()">, </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@implementation">
    <xsl:choose>
      <xsl:when test=".='simple'"/>
      <xsl:when test=".='final'"/>
      <xsl:when test=".='virtual'">virtual </xsl:when>
      <xsl:when test=".='abstract'">virtual </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Method">
    <xsl:param name="BodyOk"/>
    <xsl:variable name="TemplateOk"><xsl:if test="parent::class/@implementation='container'">ok</xsl:if></xsl:variable>
    <xsl:variable name="ClassName" select="parent::class/@name"/>
    <xsl:variable name="MethodName">
      <xsl:if test="$BodyOk='ok'">
        <xsl:value-of select="concat($ClassName,'::')"/>
      </xsl:if>
      <xsl:if test="@constructor!='no'">
        <xsl:value-of select="$ClassName"/>
      </xsl:if>
      <xsl:if test="@constructor='no'">
        <xsl:value-of select="@name"/>
      </xsl:if>
      <xsl:if test="@name='operator'">
        <xsl:if test="contains('abcdefghijklmnopqrstuvwxyz',substring(@operator,1,1))"><xsl:text> </xsl:text></xsl:if>
        <xsl:value-of select="@operator"/>
      </xsl:if>
    </xsl:variable>
    <xsl:if test="@constructor!='no'">
      <xsl:if test="$BodyOk!='ok' and $TemplateOk!='ok' and @inline='yes'">inline </xsl:if>
      <xsl:value-of select="concat($MethodName,'(')"/>
      <xsl:apply-templates select="param">
        <xsl:with-param name="BodyOk" select="$BodyOk"/>
        <xsl:with-param name="Range" select="@constructor"/>
      </xsl:apply-templates>
      <xsl:text>)</xsl:text>
			<xsl:if test="@inline='yes'">
			<xsl:apply-templates select="parent::class" mode="InlineMembers"/>
    {<xsl:apply-templates select="inline/body" xml:space="preserve"/>
    }</xsl:if>
    </xsl:if>
    <xsl:if test="@constructor='no'">
      <xsl:if test="@member='class' and $BodyOk!='ok'">static </xsl:if>
      <xsl:if test="$BodyOk!='ok' and $TemplateOk!='ok' and @inline='yes'">inline </xsl:if>
      <xsl:apply-templates select="@implementation"/>
      <xsl:apply-templates select="return/type"/>
      <xsl:apply-templates select="return/variable" mode="Typedef">
        <xsl:with-param name="Name">
          <xsl:value-of select="$MethodName"/>
          <xsl:text>(</xsl:text>
          <xsl:apply-templates select="param">
            <xsl:with-param name="BodyOk" select="$BodyOk"/>
            <xsl:with-param name="Range" select="return/variable/@range"/>
          </xsl:apply-templates>
          <xsl:text>)</xsl:text>
          <xsl:if test="@modifier='const'"> const</xsl:if>
          <xsl:if test="(@implementation='abstract' or ancestor::class/@implementation='abstract') and $BodyOk!='ok'"> = 0</xsl:if>
        </xsl:with-param>
      </xsl:apply-templates>
      <xsl:apply-templates select="exception"/>
			<xsl:if test="@inline='yes'">
    {<xsl:apply-templates select="inline/body"/>
    }</xsl:if>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="body" mode="Declaration">
    <xsl:for-each select="line">
    <xsl:text xml:space="preserve">
</xsl:text><xsl:value-of select="@value" xml:space="preserve"/>
    </xsl:for-each>
    <xsl:text xml:space="preserve">
</xsl:text>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="body">
    <xsl:for-each select="line">
    <xsl:text xml:space="preserve">
        </xsl:text><xsl:value-of select="@value" xml:space="preserve"/>
    </xsl:for-each>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="exception">
    <xsl:if test="position()=1">
            throw </xsl:if>
    <xsl:value-of select="id(@idref)/@name"/>
    <xsl:if test="position()!=last()">, </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="model" mode="FullPackageName">
	  <xsl:value-of select="@name"/>
	</xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="model" mode="TypePrefix">tmpl</xsl:template>
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
  <xsl:template name="FullIncludeName">
    <xsl:variable name="ClassID" select="@id"/>
    <xsl:variable name="PackageName" select="parent::package/@name"/>
    <xsl:apply-templates select="descendant::*/@desc" mode="ListInclude"/>
    <xsl:apply-templates select="descendant::*/@index-desc" mode="ListInclude"/>
    <xsl:apply-templates select="import[not(export or body)]" mode="ListInclude"/>
    <xsl:apply-templates select="method/exception" mode="ListInclude"/>
    <xsl:apply-templates select="inherited" mode="ListInclude"/>
    <xsl:apply-templates select="dependency[@type='interface']" mode="ListInclude"/>
    <xsl:apply-templates select="//relationship/father[@idref=$ClassID]" mode="ListInclude"/>
    <xsl:apply-templates select="//relationship/child[@idref=$ClassID]" mode="ListInclude"/>
    <xsl:apply-templates select="typedef" mode="ListInclude"/>
    <xsl:apply-templates select="property" mode="ListInclude"/>
    <xsl:apply-templates select="method/return" mode="ListInclude"/>
    <xsl:apply-templates select="method/param" mode="ListInclude"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="FullReferenceName">
    <xsl:variable name="ClassID" select="@id"/>
    <xsl:variable name="PackageName" select="parent::package/@name"/>
    <xsl:apply-templates select="dependency[@type!='interface' and @type!='body']" mode="ListRef"/>
    <xsl:apply-templates select="//relationship/father[@idref=$ClassID]" mode="ListRef"/>
    <xsl:apply-templates select="//relationship/child[@idref=$ClassID]" mode="ListRef"/>
    <xsl:apply-templates select="typedef" mode="ListRef"/>
    <xsl:apply-templates select="method/return" mode="ListRef"/>
    <xsl:apply-templates select="method/param" mode="ListRef"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef | property" mode="ListInclude">
    <xsl:call-template name="TypeListInclude">
    <xsl:with-param name="ClassID" select="parent::class/@id"/>
    </xsl:call-template>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="param | return" mode="ListInclude">
    <xsl:call-template name="TypeListInclude">
    <xsl:with-param name="ClassID" select="parent::method/parent::class/@id"/>
    </xsl:call-template>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="TypeListInclude">
    <xsl:param name="ClassID"/>
    <xsl:choose>
        <xsl:when test="type/element[@level='0' and @idref]">
          <xsl:apply-templates select="type/element[@level='0' and @idref]" mode="ListInclude">
            <xsl:with-param name="ClassID" select="$ClassID"/>
          </xsl:apply-templates>
        </xsl:when>
       <xsl:when test="type[@level='0' and @idref and not(enumvalue)]">
          <xsl:apply-templates select="type[@level='0' and @idref]" mode="ListInclude">
            <xsl:with-param name="ClassID" select="$ClassID"/>
          </xsl:apply-templates>
        </xsl:when>
     </xsl:choose>
     <xsl:if test="type/list[@idref]">
       <xsl:apply-templates select="type/list[@idref]" mode="ListInclude">
         <xsl:with-param name="ClassID" select="$ClassID"/>
       </xsl:apply-templates>
     </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type | element | @index-idref" mode="ListInclude">
    <xsl:param name="ClassID"/>
    <xsl:variable name="RefID">
      <xsl:value-of select="@idref"/>
      <xsl:if test="name()='index-idref'"><xsl:value-of select="."/></xsl:if>
    </xsl:variable>
    <xsl:choose>
       <xsl:when test="id($RefID)[self::class[@id!=$ClassID] or self::reference[@type='class' and @id!=$ClassID] or self::interface[@id!=$ClassID]]">
          <xsl:element name="reference">
            <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
            <xsl:attribute name="name"><xsl:value-of select="id($RefID)/@name"/></xsl:attribute>
            <xsl:attribute name="class"><xsl:value-of select="id($RefID)/@name"/></xsl:attribute>
            <xsl:attribute name="value">
              <xsl:apply-templates select="id($RefID)" mode="ListInclude"/>
            </xsl:attribute>
          </xsl:element>
       </xsl:when>
       <xsl:when test="id($RefID)[self::typedef[parent::class/@id!=$ClassID]]">
          <xsl:element name="reference">
            <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
            <xsl:attribute name="name"><xsl:value-of select="id($RefID)/@name"/></xsl:attribute>
            <xsl:attribute name="class"><xsl:value-of select="id($RefID)/parent::class/@name"/></xsl:attribute>
            <xsl:attribute name="value">
              <xsl:apply-templates select="id($RefID)/parent::class" mode="ListInclude"/>
            </xsl:attribute>
          </xsl:element>
        </xsl:when>
        <xsl:when test="id($RefID)[self::reference[@type='typedef']]">
          <xsl:element name="reference">
            <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
            <xsl:attribute name="name"><xsl:value-of select="id($RefID)/@name"/></xsl:attribute>
            <xsl:attribute name="class"><xsl:value-of select="id($RefID)/@class"/></xsl:attribute>
            <xsl:attribute name="value">
              <xsl:apply-templates select="id($RefID)" mode="ListInclude"/>
            </xsl:attribute>
          </xsl:element>
        </xsl:when>
     </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="exception" mode="ListInclude">
    <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="id(@idref)/@name"/>
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:value-of select="id(@idref)/@name"/>
      </xsl:attribute>
      <xsl:attribute name="package">
        <xsl:value-of select="id(@idref)/parent::package/@name"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:apply-templates select="id(@idref)" mode="ListInclude"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child" mode="ListInclude">
    <xsl:if test="preceding-sibling::father/@level='0' and preceding-sibling::father/@range!='no'">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="id(preceding-sibling::father/@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="id(preceding-sibling::father/@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="id(preceding-sibling::father/@idref)/parent::package/@name"/>
          <xsl:value-of select="id(preceding-sibling::father/@idref)/@package"/>
        </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:apply-templates select="id(preceding-sibling::father/@idref)" mode="ListInclude"/>
     </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="preceding-sibling::father/list[@idref] and preceding-sibling::father/@range!='no'">
    <xsl:apply-templates select="preceding-sibling::father/list" mode="ListInclude">
      <xsl:with-param name="ClassID" select="@idref"/>
    </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="father" mode="ListInclude">
    <xsl:if test="following-sibling::child/@level='0'">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="id(following-sibling::child/@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="id(following-sibling::child/@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="id(following-sibling::child/@idref)/@package"/>
          <xsl:value-of select="id(following-sibling::child/@idref)/parent::package/@name"/>
        </xsl:attribute>
        <xsl:attribute name="value">
          <xsl:apply-templates select="id(following-sibling::child/@idref)" mode="ListInclude"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="following-sibling::child/list[@idref]">
      <xsl:variable name="ClassID" select="@idref"/>
      <xsl:apply-templates select="following-sibling::child/list" mode="ListInclude">
        <xsl:with-param name="ClassID" select="$ClassID"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="list" mode="ListInclude">
    <xsl:param name="ClassID"/>
    <xsl:if test="@type='indexed' and @index-idref and @level='0'">
      <xsl:apply-templates select="@index-idref" mode="ListInclude">
        <xsl:with-param name="ClassID" select="$ClassID"/>
      </xsl:apply-templates>
    </xsl:if>
    <xsl:if test="id(@idref)[self::class or self::reference[@type='class'] or self::interface]">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="id(@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="id(@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="id(@idref)/parent::package/@name"/>
          <xsl:value-of select="id(@idref)/@package"/>
        </xsl:attribute>
        <xsl:attribute name="value">
          <xsl:apply-templates select="id(@idref)" mode="ListInclude"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="list" mode="ListRef">
    <xsl:param name="ClassID"/>
    <xsl:if test="@type='indexed' and id(@index-idref)[self::class or self::reference[@type='class'] or self::interface] and @level!='0'">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="id(@index-idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="id(@index-idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="id(@index-idref)/parent::package/@name"/>
          <xsl:value-of select="id(@index-idref)/@package"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef" mode="ListRef">
    <xsl:variable name="ClassID" select="parent::class/@id"/>
    <xsl:variable name="RefID" select="type/@idref"/>
    <xsl:if test="$RefID!=$ClassID and id($RefID)[self::class or self::reference[@type='class'] or self::interface] and type/@level!='0'">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="id($RefID)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="id($RefID)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="id($RefID)/parent::package/@name"/>
          <xsl:value-of select="id($RefID)/@package"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="type/element">
      <xsl:apply-templates select="type/element" mode="ListRef"/>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="param | return" mode="ListRef">
    <xsl:variable name="ClassID" select="parent::method/parent::class/@id"/>
    <xsl:variable name="RefID" select="type/@idref"/>
    <xsl:if test="$RefID!=$ClassID and id($RefID)[self::class or self::reference[@type='class'] or self::interface] and type/@level!='0'">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="id($RefID)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="id($RefID)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="id($RefID)/parent::package/@name"/>
          <xsl:value-of select="id($RefID)/@package"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="type/element">
      <xsl:apply-templates select="type/element" mode="ListRef"/>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="element" mode="ListRef">
    <xsl:variable name="ClassID" select="ancestor::class/@id"/>
    <xsl:variable name="RefID" select="@idref"/>
    <xsl:if test="$RefID!=$ClassID and id($RefID)[self::class or self::reference[@type='class'] or self::interface] and @level!='0'">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="id($RefID)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="id($RefID)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="id($RefID)/parent::package/@name"/>
          <xsl:value-of select="id($RefID)/@package"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child" mode="ListRef">
    <xsl:if test="preceding-sibling::father/@level!='0' and preceding-sibling::father/@range!='no'">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="id(preceding-sibling::father/@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="id(preceding-sibling::father/@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="id(preceding-sibling::father/@idref)/@package"/>
          <xsl:value-of select="id(preceding-sibling::father/@idref)/parent::package/@name"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="preceding-sibling::father[@range!='no']/list[@idref]">
      <xsl:variable name="ClassID" select="@idref"/>
      <xsl:apply-templates select="preceding-sibling::father[@range!='no']/list" mode="ListRef">
        <xsl:with-param name="ClassID" select="$ClassID"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="father" mode="ListRef">
    <xsl:if test="following-sibling::child/@level!='0'">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="id(following-sibling::child/@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="class">
          <xsl:value-of select="id(following-sibling::child/@idref)/@name"/>
        </xsl:attribute>
        <xsl:attribute name="package">
          <xsl:value-of select="id(following-sibling::child/@idref)/@package"/>
          <xsl:value-of select="id(following-sibling::child/@idref)/parent::package/@name"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="following-sibling::child/list[@idref]">
      <xsl:variable name="ClassID" select="@idref"/>
      <xsl:apply-templates select="following-sibling::child/list" mode="ListRef">
        <xsl:with-param name="ClassID" select="$ClassID"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited" mode="ListInclude">
    <xsl:element name="reference">
      <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="id(@idref)/@name"/>
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:value-of select="id(@idref)/@name"/>
      </xsl:attribute>
      <xsl:attribute name="package">
        <xsl:value-of select="id(@idref)/parent::package/@name"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:apply-templates select="id(@idref)" mode="ListInclude"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="dependency" mode="ListInclude">
    <xsl:element name="reference">
      <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="id(@idref)/@name"/>
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:value-of select="id(@idref)/@name"/>
      </xsl:attribute>
      <xsl:attribute name="type">
        <xsl:value-of select="@type"/>
      </xsl:attribute>
      <xsl:attribute name="package">
        <xsl:value-of select="id(@idref)/parent::package/@name"/>
        <xsl:value-of select="id(@idref)/@package"/><!-- Import -->
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:apply-templates select="id(@idref)" mode="ListInclude"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="dependency" mode="ListRef">
    <xsl:element name="reference">
      <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="id(@idref)/@name"/>
      </xsl:attribute>
      <xsl:attribute name="class">
        <xsl:value-of select="id(@idref)/@name"/>
      </xsl:attribute>
      <xsl:attribute name="type">
        <xsl:value-of select="@type"/>
      </xsl:attribute>
      <xsl:attribute name="package">
        <xsl:value-of select="id(@idref)/parent::package/@name"/>
        <xsl:value-of select="id(@idref)/@package"/><!-- Import -->
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@desc | @index-desc" mode="ListInclude">
    <xsl:variable name="Type" select="."/>
    <xsl:variable name="FinalType" select="document($Language)//type[@name=$Type]/@import"/>
    <xsl:if test="string-length($FinalType)!=0">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name(parent::*)"/></xsl:attribute>
        <xsl:attribute name="name"><xsl:value-of select="document($Language)//type[@name=$Type]/@name"/></xsl:attribute>
        <xsl:attribute name="class"><xsl:value-of select="document($Language)//type[@name=$Type]/@name"/></xsl:attribute>
        <xsl:attribute name="package"><xsl:value-of select="document($Language)//type[@name=$Type]/@package"/></xsl:attribute>
        <xsl:attribute name="value">&lt;<xsl:value-of select="$FinalType"/>&gt;</xsl:attribute>
      </xsl:element>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="ListInclude">"<xsl:value-of select="concat(@name,'.h')"/>"</xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="import" mode="ListInclude">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="@name"/>
        </xsl:attribute>
        <xsl:attribute name="value">
        <xsl:value-of select="concat('&lt;',@param,'&gt;')"/>
        </xsl:attribute>
      </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference | interface" mode="ListInclude">
    <xsl:choose>
      <xsl:when test="ancestor::import[@param!='']">&lt;<xsl:value-of select="ancestor::import/@param"/>&gt;</xsl:when>
      <xsl:otherwise>
        <xsl:if test="@class">"<xsl:value-of select="concat(@class,'.h')"/>"</xsl:if>
        <xsl:if test="not(@class)">"<xsl:value-of select="concat(@name,'.h')"/>"</xsl:if>
      </xsl:otherwise>
    </xsl:choose>
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
    <xsl:if test="string-length($Class)!=0"><xsl:value-of select="concat($Class,'::')"/></xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="FullPackageName">
    <xsl:param name="CurrentClassName"/>
    <xsl:param name="CurrentPackageName"/>
    <!--xsl:value-of select="concat('class=',@name,'|',$CurrentPackageName,'::',$CurrentClassName)"/-->
    <xsl:variable name="RefPackageName" select="parent::package/@name"/>
    <xsl:if test="$RefPackageName!=$CurrentPackageName or (string-length($CurrentPackageName)=0 and string-length($RefPackageName)!=0)">
      <xsl:value-of select="concat(parent::package/@name,'::')"/>
    </xsl:if>
    <xsl:if test="@name!=$CurrentClassName or string-length($CurrentClassName)=0">
    <xsl:value-of select="@name"/>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="enumvalue" mode="FullPackageName">
    <xsl:param name="CurrentClassName"/>
    <xsl:param name="CurrentPackageName"/>
    <!--xsl:value-of select="concat('enumvalue=',$CurrentPackageName,'::',$CurrentClassName)"/-->
    <xsl:variable name="Class">
      <xsl:apply-templates select="ancestor::class" mode="FullPackageName">
        <xsl:with-param name="CurrentClassName" select="$CurrentClassName"/>
        <xsl:with-param name="CurrentPackageName" select="$CurrentPackageName"/>
      </xsl:apply-templates>
    </xsl:variable>
    <xsl:if test="string-length($Class)!=0">
      <xsl:value-of select="concat($Class,'::')"/>
    </xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>

  <!-- ======================================================================= -->
  <xsl:template match="reference | interface" mode="Package">
    <!--xsl:value-of select="ancestor::import/@param"/>
    <xsl:if test="string-length(ancestor::import/@param)!=0 and string-length(@package)!=0">.</xsl:if-->
    <xsl:value-of select="@package"/>
  </xsl:template>

  <!-- ======================================================================= -->
  <xsl:template match="reference | interface" mode="FullPackageName">
    <xsl:param name="CurrentClassName"/>
    <xsl:param name="CurrentPackageName"/>

    <xsl:variable name="FullPackageName">
      <xsl:apply-templates select="." mode="Package"/>
    </xsl:variable>

    <xsl:if test="$FullPackageName!=$CurrentPackageName or string-length($CurrentPackageName)=0">
      <xsl:value-of select="concat($FullPackageName,'::')"/>
    </xsl:if>
    <xsl:if test="@type='typedef'">
      <xsl:value-of select="concat(@class,'::')"/>
    </xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="FullReferenceName">
    <xsl:param name="CurrentPackageName"/>
    <xsl:variable name="PackageOk">
      <xsl:if test="(parent::package/@name!=$CurrentPackageName) or (string-length($CurrentPackageName)=0 and string-length(parent::package/@name)!=0)">ok</xsl:if>
    </xsl:variable>
    <xsl:if test="$PackageOk='ok'">
      <xsl:value-of select="concat('namespace ',parent::package/@name)"/>
      <xsl:text>
{</xsl:text>
    </xsl:if>
    <xsl:value-of select="concat('class',@name)"/>;
    <xsl:if test="$PackageOk='ok'">
}
</xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@value">
    <xsl:value-of select="."/>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="return" mode="Comment">
    <xsl:if test="type[@desc!='void'] or not(type/@desc)">
     \return <xsl:value-of select="comment/text()"/>
    </xsl:if>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="param" mode="Comment">
     \param <xsl:value-of select="concat(@name,' ',comment)"/>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="comment">
    <xsl:variable name="Brief"><xsl:if test="string-length(@brief)=0"><xsl:value-of select="text()"/></xsl:if><xsl:value-of select="@brief"/></xsl:variable>
    <xsl:variable name="Detail"><xsl:if test="string-length(@brief)&gt;0"><xsl:value-of select="text()"/></xsl:if></xsl:variable>
    //! <xsl:value-of select="$Brief"/>
    <xsl:if test="string-length(@brief)&gt;0">

    /*! <xsl:value-of select="$Detail"/>*/</xsl:if>
    </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="comment" mode="Method">
    //! <xsl:value-of select="@brief"/>

    /*! <xsl:value-of select="text()"/>
    </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="comment" mode="Details">
    //! <xsl:value-of select="text()"/>
    </xsl:template>
  <!-- ======================================================================= -->
</xsl:stylesheet>








