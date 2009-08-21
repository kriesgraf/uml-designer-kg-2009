<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ======================================================================= -->
  <xsl:variable name="Language">
    <xsl:if test="$LanguageFolder=''">
      <xsl:message terminate="yes">Parameter $LanguageFolder not yet filled!</xsl:message>
    </xsl:if>
    <xsl:value-of select="$LanguageFolder"/>
    <xsl:text>\LanguageVbasic.xml</xsl:text>
  </xsl:variable>
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
  <xsl:template match="type" mode="TypePrefix">
    <xsl:apply-templates select="@level" mode="TypePrefix"/>
    <xsl:apply-templates select="@desc" mode="TypePrefix"/>
    <xsl:apply-templates select="id(@idref)" mode="TypePrefix"/>
    <xsl:apply-templates select="list | enumvalue[1] | element[1]" mode="TypePrefix"/>
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
       <!--xsl:value-of select="concat('[',name(),'=',.,'PointerOk=',$PointerOk,'ArrayOk=',$ArrayOk,']')"/-->
    <xsl:choose>
      <xsl:when test="name()='enumvalue'">e</xsl:when>
      <xsl:otherwise>
          <xsl:value-of select="document($Language)//type[@name=current()]/@prefix"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@range | @constructor" mode="Code">
    <xsl:choose>
      <xsl:when test=".='private'">Private </xsl:when>
      <xsl:when test=".='protected'">Protected </xsl:when>
      <xsl:when test=".='public'">Public </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="property" mode="Code">
    <xsl:variable name="ConstOk">
      <xsl:if test="type/@modifier='const'">ok</xsl:if>
    </xsl:variable>
    <xsl:if test="get[@range='no'] and set[@range='no']">
      <xsl:apply-templates select="comment" mode="Simple"/>
      <xsl:text>
    </xsl:text>      
    </xsl:if>
    <xsl:apply-templates select="variable/@range" mode="Code"/>
	<xsl:if test="@behaviour='WithEvents'">WithEvents </xsl:if>
    <xsl:if test="@member='class'">Shared </xsl:if>
    <xsl:if test="$ConstOk='ok'">Const </xsl:if>
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
    </xsl:if><xsl:text>
    </xsl:text>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="variable" mode="Typedef">
    <xsl:param name="PropertyOk"/>
    <xsl:param name="Type"/>
    <xsl:param name="Name"/>
    <xsl:value-of select="$Name"/>
    <xsl:if test="$Type!='Sub'">
      <xsl:if test="@sizeref or @size"><xsl:text>(</xsl:text>
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
      <xsl:text>)</xsl:text></xsl:if>
      <xsl:text> As </xsl:text><xsl:value-of select="$Type"/>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type">
    <xsl:choose>
      <xsl:when test="enumvalue">enum<xsl:if test="parent::property"><xsl:value-of select="concat(' ', PrefixEnumProperty, parent::property/@name)"/></xsl:if><xsl:apply-templates select="enumvalue" mode="Code"/></xsl:when>
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
    <xsl:if test="@type='indexed'"><xsl:value-of select="$Index"/>, </xsl:if>
    <xsl:value-of select="$Type"/>
    <xsl:text>)</xsl:text>
   </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="array" mode="Typedef">
    <xsl:param name="Type"/>
    <xsl:value-of select="$Type"/>
    <xsl:if test="@size">[<xsl:value-of select="@size"/>]</xsl:if>
    <xsl:if test="@sizeref">[<xsl:apply-templates select="id(@sizeref)" mode="FullPackageName"/>]</xsl:if>
</xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="param" mode="Code">
    <xsl:variable name="By">
	  <xsl:choose>
	    <xsl:when test="type/@by='val'">ByVal </xsl:when>
  	    <xsl:when test="type/@by='ref'">ByRef </xsl:when>
	  </xsl:choose>
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
  <xsl:template match="@implementation" mode="Code">
    <xsl:param name="Implementation"/>
    <xsl:choose>
      <xsl:when test=".='final'">NotOverridable Overrides </xsl:when>
      <xsl:when test=".='virtual'">Overrides </xsl:when>
      <xsl:when test=".='root'">Overridable </xsl:when>
      <xsl:when test=".='abstract' and $Implementation!='abstract'">MustOverride </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Method">
    <xsl:param name="Implementation"/>
    <xsl:param name="ImplementedMethods"/>
    <xsl:variable name="TemplateOk"><xsl:if test="parent::class/@implementation='container'">ok</xsl:if></xsl:variable>
    <xsl:variable name="MethodName">
	  <xsl:choose>
	    <xsl:when test="@constructor!='no'"><xsl:apply-templates select="@constructor" mode="Code"/>Sub New</xsl:when>
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
      <xsl:text>)</xsl:text>
    End Sub
    </xsl:if>
    <xsl:if test="@constructor='no'">
      <xsl:variable name="Type">
        <xsl:apply-templates select="return/type"/>
      </xsl:variable>
      <xsl:variable name="Prefix">
        <xsl:choose>
          <xsl:when test="$Type='Sub'">Sub </xsl:when>
          <xsl:when test="$Type!='Sub' and @name='operator'">Operator </xsl:when>
          <xsl:otherwise>Function </xsl:otherwise>
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
      <xsl:if test="@member='class'">Shared </xsl:if>
      <xsl:value-of select="$Prefix"/>
      <xsl:apply-templates select="return/variable" mode="Typedef">
        <xsl:with-param name="Name">
          <xsl:value-of select="$MethodName"/>
          <xsl:text>(</xsl:text>
          <xsl:apply-templates select="param" mode="Code">
            <xsl:with-param name="Range" select="return/variable/@range"/>
          </xsl:apply-templates>
          <xsl:text>)</xsl:text>
        </xsl:with-param>
        <xsl:with-param name="Type" select="$Type"/>
      </xsl:apply-templates>
      <xsl:if test="@overrides">
        <xsl:if test="contains($ImplementedMethods,concat(@overrides,';'))">
          <xsl:text> Implements </xsl:text>
          <xsl:value-of select="id(@overrides)/@name"/>.<xsl:value-of select="@name"/>
        </xsl:if>
      </xsl:if>
      <xsl:if test="@implementation!='abstract'">
    End <xsl:value-of select="$Prefix"/>
      </xsl:if>
	  <xsl:text>
    </xsl:text>
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
  <xsl:template match="body" mode="Code">
    <xsl:for-each select="line">
    <xsl:text xml:space="preserve">
</xsl:text><xsl:value-of select="@value" xml:space="preserve"/>
    </xsl:for-each>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="exception" mode="Code">
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
          <xsl:apply-templates select="type/element[@idref]" mode="ListImports">
            <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
          </xsl:apply-templates>
        </xsl:when>
       <xsl:when test="type[@idref and not(enumvalue)]">
          <xsl:apply-templates select="type[@idref]" mode="ListImports">
            <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
          </xsl:apply-templates>
        </xsl:when>
     </xsl:choose>
     <xsl:if test="type/list[@idref]">
       <xsl:apply-templates select="type/list[@idref]" mode="ListImports">
         <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
       </xsl:apply-templates>
     </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type | element | @index-idref" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:variable name="RefID">
      <xsl:value-of select="@idref"/>
      <xsl:if test="name()='index-idref'"><xsl:value-of select="."/></xsl:if>
    </xsl:variable>
    <xsl:apply-templates select="id($RefID)" mode="ClassImports">
       <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
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
    <xsl:apply-templates select="id(preceding-sibling::father/@idref)" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="preceding-sibling::father/list" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="father" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:apply-templates select="id(following-sibling::child/@idref)" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="following-sibling::child/list" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="list" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:apply-templates select="id(@index-idref)" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="id(@idref)" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:apply-templates select="id(@idref)" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="dependency" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:apply-templates select="id(@idref)" mode="ListImports">
      <xsl:with-param name="CurrentPackage" select="$CurrentPackage"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@desc | @index-desc" mode="ListImports">
    <xsl:variable name="Type" select="."/>
    <xsl:variable name="FinalType" select="document($Language)//type[@name=$Type]/@import"/>
    <xsl:if test="string-length($FinalType)!=0">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name(parent::*)"/></xsl:attribute>
        <xsl:attribute name="name"><xsl:value-of select="document($Language)//type[@name=$Type]/@name"/></xsl:attribute>
        <xsl:attribute name="class"><xsl:value-of select="document($Language)//type[@name=$Type]/@name"/></xsl:attribute>
        <xsl:attribute name="package"><xsl:value-of select="document($Language)//type[@name=$Type]/@package"/></xsl:attribute>
        <xsl:attribute name="value"><xsl:value-of select="$FinalType"/></xsl:attribute>
      </xsl:element>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="Package">
    <xsl:apply-templates select="parent::*" mode="Package"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="package" mode="Package">
    <xsl:if test="parent::*">
      <xsl:apply-templates select="parent::*" mode="Package"/>.</xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="root" mode="Package"><xsl:value-of select="@name"/></xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference | interface" mode="Package">
    <xsl:value-of select="ancestor::import/@param"/>
    <xsl:if test="string-length(ancestor::import/@param)!=0 and string-length(@package)!=0">.</xsl:if>
    <xsl:value-of select="@package"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="import" mode="ListImports">
      <xsl:element name="reference">
        <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
        <xsl:attribute name="name">
          <xsl:value-of select="@name"/>
        </xsl:attribute>
        <xsl:attribute name="value">
        <xsl:value-of select="@param"/>
        </xsl:attribute>
      </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference | interface" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:variable name="TargetPackage">
      <xsl:choose>
        <xsl:when test="string-length(@package)!=0"><xsl:value-of select="@package"/></xsl:when>
        <xsl:when test="string-length(ancestor::import/@param)!=0"><xsl:value-of select="ancestor::import/@param"/></xsl:when>
        <xsl:otherwise>UnknownPackage</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="$CurrentPackage!=$TargetPackage">
    <xsl:element name="reference">
      <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="package">
        <xsl:value-of select="$TargetPackage"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:apply-templates select="." mode="Package"/>
      </xsl:attribute>
    </xsl:element>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="ListImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:if test="$CurrentPackage!=parent::*/@name">
    <xsl:element name="reference">
      <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="package">
        <xsl:value-of select="parent::*/@name"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:apply-templates select="." mode="Package"/>
      </xsl:attribute>
    </xsl:element>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef" mode="ClassImports">
    <xsl:param name="CurrentPackage"/>
    <xsl:if test="$CurrentPackage!=parent::class/parent::*/@name">
    <xsl:element name="reference">
      <xsl:attribute name="node"><xsl:value-of select="name()"/></xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="package">
        <xsl:value-of select="parent::class/parent::*/@name"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:apply-templates select="parent::class" mode="Package"/>
      </xsl:attribute>
    </xsl:element>
    </xsl:if>
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
    <xsl:if test="string-length($Class)!=0"><xsl:value-of select="concat($Class,'.')"/></xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class" mode="FullPackageName">
    <xsl:param name="CurrentClassName"/>
    <xsl:param name="CurrentPackageName"/>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="enumvalue" mode="FullPackageName">
    <xsl:param name="CurrentClassName"/>
    <xsl:param name="CurrentPackageName"/>
    <xsl:if test="ancestor::class/@name!=$CurrentClassName">
      <xsl:value-of select="concat(ancestor::class/@name, '.')"/>
    </xsl:if>
    <xsl:value-of select="concat(ancestor::typedef/@name,'.',@name)"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference |interface" mode="FullPackageName">
    <xsl:param name="CurrentClassName"/>
    <xsl:param name="CurrentPackageName"/>
    <xsl:if test="@type='typedef'">
      <xsl:value-of select="concat(@class,'.')"/>
    </xsl:if>
    <xsl:value-of select="@name"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="@value">
    <xsl:value-of select="."/>
  </xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="return" mode="Comment">
    ''' &lt;returns&gt;<xsl:value-of select="comment/text()"/>&lt;/returns&gt;</xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="param" mode="Comment">
    ''' &lt;param name="<xsl:value-of select="@name"/>"&gt;<xsl:value-of select="comment"/>&lt;/param&gt;</xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="comment" mode="Comment">
    <xsl:variable name="Brief"><xsl:if test="string-length(@brief)=0"><xsl:value-of select="text()"/></xsl:if><xsl:value-of select="@brief"/></xsl:variable>
    <xsl:variable name="Detail"><xsl:if test="string-length(@brief)&gt;0"><xsl:value-of select="text()"/></xsl:if></xsl:variable>
    ''' &lt;summary&gt;
    ''' <xsl:value-of select="$Brief"/>
    ''' &lt;/summary&gt;
    ''' &lt;remarks&gt;<xsl:value-of select="$Detail"/>&lt;/remarks&gt;</xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="method" mode="Comment">

    ''' &lt;summary&gt;
    ''' <xsl:value-of select="comment/@brief"/>
    ''' &lt;/summary&gt;<xsl:apply-templates select="param" mode="Comment"/>
	<xsl:apply-templates select="return[type/@desc!='void']" mode="Comment"/>
    ''' &lt;remarks&gt;<xsl:value-of select="comment/text()"/>&lt;/remarks&gt;</xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="comment" mode="Simple">
    ''' &lt;summary&gt;<xsl:value-of select="text()"/>&lt;/summary&gt;</xsl:template>
<!-- ======================================================================= -->
  <xsl:template match="enumvalue | element" mode="Simple">
        ''' &lt;summary&gt;<xsl:value-of select="text()"/>&lt;/summary&gt;</xsl:template>
  <!-- ======================================================================= -->
</xsl:stylesheet>



















