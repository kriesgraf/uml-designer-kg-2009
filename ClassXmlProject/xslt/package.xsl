<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<xsl:output method="html" indent="yes"/>
	<!-- ======================================================================= -->
	<xsl:param name="IconsFolder">./</xsl:param>
	<!-- ======================================================================= -->
    <xsl:key match="reference" name="package" use="@package"/>
	<!-- ======================================================================= -->
	<xsl:variable name="Separator">
	  <xsl:choose>
    	<xsl:when test="/root/generation/@language='0'">::</xsl:when>
	    <xsl:when test="/root/generation/@language='2'">.</xsl:when>
    	<xsl:otherwise/>
      </xsl:choose>
    </xsl:variable>
	<!-- ======================================================================= -->
	<xsl:template match="/">
		<xsl:apply-templates select="//relationship[1]"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="*">
		<xsl:call-template name="Header"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Header">
		<html>
			<head>
				<title>
					<xsl:value-of select="//root/@name"/>: <xsl:value-of select="//root/comment"/></title>
				<style>body
{
    background:white;
    color:black;
	font-style: normal;
	font-size: 100%;
}
table.border
{
border-left: 1px solid gray;
  border-right: 3px solid black;
  border-top: 1px solid gray;
  border-bottom: 3px solid black;
}
td   {background:white; color:black;
  }
td   {background:white; color:black;
  }
th   {background:white; color:black;
border-collapse: collapse
}
.code
{
border-left: 1px solid gray;
  border-right: 3px solid black;
  border-top: 1px solid gray;
  border-bottom: 3px solid black;
	font-style: normal;
	font-size: 100%;
	color:black;
}
.member   {background:white; color:black;
  border-bottom: 1px solid black;
  }
.package
{
	font-style: normal;
	font-weight: bold;
	font-size: 100%;
	color:blue;
}
.template
{
	font-style: normal;
	font-weight: bold;
	font-size: 100%;
	color:blue;
}
.class-package
{
	font-style: normal;
	font-size: 90%;
	color:magenta;
}
.comment
{
	font-style: normal;
	font-size: 90%;
	color:green;
}
.small
{
	font-style: normal;
	font-size: 80%;
	color:black;
}</style>
			</head>
			<body>
				<xsl:choose>
					<xsl:when test="name()='root' or name()='package'">
						<xsl:apply-templates select="." mode="Package"/>
					</xsl:when>
					<xsl:when test="name()='import'">
						Import <xsl:apply-templates select="." mode="Import"/>
					</xsl:when>
					<xsl:when test="name()='relationship'">
						<table>
							<xsl:apply-templates select="//relationship" mode="Relationship"/>
						</table>
					</xsl:when>
					<xsl:when test="name()='collaboration'">
						<table>
							<xsl:apply-templates select="id(@idref)" mode="Relationship"/>
						</table>
					</xsl:when>
					<xsl:when test="name()='class'">
						<xsl:apply-templates select="." mode="Class"/>
						<p/>
							<xsl:if test="typedef"><span class="class-package">
								<xsl:value-of select="@name"/>
							</span> typedef specification:<p/>
						<xsl:apply-templates select="typedef" mode="Typedef"/></xsl:if>
					</xsl:when>
					<xsl:when test="name()='exception'">
						<xsl:apply-templates select="." mode="Exception"/>
					</xsl:when>
					<xsl:when test="name()='method'">
						<p class="comment">
							<xsl:value-of select="comment/@brief"/>
						</p>
						<p>
							<span class="class-package">
								<xsl:value-of select="parent::class/@name"/>
							</span>
							<xsl:value-of select="concat(' ',name(),' specification:')"/></p>
						<xsl:if test="@inline='yes'">&lt;inline&gt;</xsl:if>
						<xsl:if test="not(@name)">&lt;Constructor&gt;</xsl:if>
						<p class="comment">
							<xsl:value-of select="comment"/>
						</p>
						<xsl:apply-templates select="return" mode="Typedef"/>
						<xsl:if test="exception">
							<p>exceptions raised:</p>
							<xsl:apply-templates select="exception" mode="Method"/>
						</xsl:if>
						<xsl:if test="param">
							<p>param specification:</p>
							<xsl:apply-templates select="param" mode="Typedef"/>
						</xsl:if>
					</xsl:when>
					<xsl:when test="name()='typedef' or name()='property' or name()='param' or name()='method'">
						<p>
							<span class="class-package">
								<xsl:value-of select="parent::class/@name"/>
							</span>
							<xsl:value-of select="concat(' ',name(),' specification:')"/></p>
						<xsl:apply-templates select="." mode="Typedef"/>
					</xsl:when>
				</xsl:choose>
			</body>
		</html>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="root" mode="Import">
		<p>Import libraries in project <span class="package"><xsl:value-of select="@name"/></span></p>
		<xsl:apply-templates select="import" mode="Import"/>
		<p/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="package" mode="Import">
		<p>Import libraries in package <span class="package"><xsl:value-of select="@name"/></span></p>
		<xsl:apply-templates select="import" mode="Import"/>
		<p/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="class" mode="Import">
		<p>Import libraries in class <span class="class-package"><xsl:value-of select="@name"/></span></p>
		<xsl:apply-templates select="import" mode="Import"/>
		<p/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="root" mode="Package">
		<table border="0">
			<tr>
				<td class="package">
					<xsl:value-of select="@name"/>
				</td>
				<td colspan="2">: <xsl:value-of select="comment/@brief"/></td>
			</tr>
			<xsl:apply-templates select="class" mode="Package"/>
			<tr>
				<td/>
				<td/>
				<td>
					<xsl:apply-templates select="package" mode="Package">
                      <xsl:with-param name="View">Nok</xsl:with-param>
                    </xsl:apply-templates>
				</td>
			</tr>
		</table>
		<xsl:apply-templates select="." mode="PackageView"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="root" mode="PackageView">
  	  <p>External dependencies:</p>
      <table border="0">
        <tr>
          <td valign="top"><xsl:apply-templates select="." mode="PackageImg"/></td>
          <td><xsl:call-template name="ExternalPackageView"/></td>
        </tr>
  	  </table>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="package" mode="PackageView">
  	  <p>Package dependencies:</p>
      <table border="0">
        <tr>
  	      <td><xsl:apply-templates select="." mode="PackageImg"/></td>
          <td><xsl:call-template name="PackageView"/></td>
        </tr>
  	  </table>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="package" mode="Package">
        <xsl:param name="View"/>
		<table border="0">
			<tr>
				<td class="package">
					<xsl:value-of select="@name"/>
				</td>
				<td colspan="2">: <xsl:value-of select="comment/@brief"/></td>
			</tr>
			<tr>
				<td/>
				<td/>
				<td>
					<xsl:apply-templates select="package" mode="Package"/>
				</td>
			</tr>
			<xsl:apply-templates select="class" mode="Package"/>
		</table>
		<xsl:if test="$View=''">
          <xsl:apply-templates select="." mode="PackageView"/>
        </xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="class" mode="Package">
		<tr>
			<td/>
			<td class="class-package">
				<xsl:value-of select="concat('&lt;',@implementation,'&gt;',@name)"/>
			</td>
			<td>: <xsl:value-of select="comment/@brief"/></td>
			<td/>
		</tr>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="import" mode="Import">
		<xsl:value-of select="@name"/> (<span class="class-package"><xsl:value-of select="@param"/></span>)<br/>
		<xsl:if test="export">
			<li>
				Reference list: <span class="comment"><xsl:apply-templates select="export/*" mode="Import"/></span></li>
		</xsl:if>
		<br/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="reference | interface" mode="Import">
		<xsl:value-of select="@name"/>
		<xsl:if test="position()!=last()">,</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="inherited">
		<td>
			<table border="0">
				<tr>
					<xsl:apply-templates select="id(@idref)" mode="Inherited"/>
				</tr>
			</table>
		</td>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="inherited" mode="Child">
		<td>
			<table border="0">
				<tr>
					<td>
						<xsl:apply-templates select="parent::class" mode="SimpleClass"/>
					</td>
				</tr>
			</table>
		</td>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="inherited" mode="Arrow">
		<td align="center">
			<img src="{$IconsFolder}Inherited.gif"/>
		</td>
		<!--td align="center">^<br/>|<br/>|<br/></td-->
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="class | reference" mode="Inherited">
		<td>
			<xsl:apply-templates select="." mode="SimpleClass"/>
		</td>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="dependency" mode="Dependencies">
		<tr class="small">
			<td align="left">
				<table>
					<tr>
						<td>
							<img src="{$IconsFolder}HorizD.GIF"/>
						</td>
						<td>[<xsl:value-of select="@action"/>]<br/><xsl:value-of select="@type"/></td>
						<td>
							<img src="{$IconsFolder}LeftD.GIF"/>
						</td>
					</tr>
				</table>
			</td>
			<td>
				<xsl:apply-templates select="id(@idref)" mode="SimpleClass"/>
			</td>
		</tr>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="dependency" mode="ReverseDependencies">
		<tr class="small">
			<td align="left">
				<table>
					<tr>
						<td>
							<img src="{$IconsFolder}RightD.GIF"/>
						</td>
						<td>[<xsl:value-of select="@action"/>]<br/><xsl:value-of select="@type"/></td>
						<td>
							<img src="{$IconsFolder}HorizD.GIF"/>
						</td>
					</tr>
				</table>
			</td>
			<td>
				<xsl:apply-templates select="parent::class" mode="SimpleClass"/>
			</td>
		</tr>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="relationship" mode="Relationship">
		<xsl:apply-templates select="father" mode="Class">
			<xsl:with-param name="SourceOk">Ok</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="father" mode="Class">
		<xsl:param name="SourceOk"/>
		<tr class="small">
			<xsl:if test="$SourceOk='Ok'">
				<td>
					<xsl:apply-templates select="id(@idref)" mode="SimpleClass"/>
				</td>
			</xsl:if>
			<td align="left">
				<table>
					<tr>
						<td>
							<xsl:apply-templates select="@cardinal"/>
							<br/>
							<xsl:value-of select="@range"/>
							<br/>
							<xsl:if test="list/@member='class' or array/@member='class'">static</xsl:if>
							<xsl:if test="@range!='no'">
								<xsl:apply-templates select="." mode="Format"/>
							</xsl:if>
						</td>
						<td>
							<xsl:if test="@range!='no'">
								<img src="{$IconsFolder}Right.GIF"/>
							</xsl:if>
							<xsl:if test="@range='no'">
								<img src="{$IconsFolder}Horizontal.GIF"/>
							</xsl:if>
						</td>
						<td align="center">[<xsl:value-of select="parent::relationship/@action"/>]<br/>
							<xsl:value-of select="parent::relationship/@type"/>
						</td>
						<td>
							<img src="{$IconsFolder}Left.GIF"/>
						</td>
						<td align="right">
							<xsl:apply-templates select="following-sibling::child/@cardinal"/>
							<br/>
							<xsl:value-of select="following-sibling::child/@range"/>
							<br/>
							<xsl:if test="following-sibling::child/list/@member='class' or following-sibling::child/array/@member='class'">
								<u>static</u>
								<xsl:text xml:space="preserve"> </xsl:text>
							</xsl:if>
							<xsl:apply-templates select="following-sibling::child" mode="Format"/>
						</td>
					</tr>
				</table>
			</td>
			<td align="center">
				<xsl:value-of select="following-sibling::child/@name"/>
				<br/>
				<xsl:apply-templates select="following-sibling::child" mode="SimpleClass"/>
			</td>
		</tr>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="child" mode="Class">
		<xsl:param name="SourceOk"/>
		<tr class="small">
			<xsl:if test="$SourceOk='Ok'">
				<td>
					<xsl:apply-templates select="id(@idref)" mode="SimpleClass"/>
				</td>
			</xsl:if>
			<td align="left">
				<table>
					<tr>
						<td>
							<xsl:apply-templates select="@cardinal"/>
							<br/>
							<xsl:value-of select="@range"/>
							<br/>
							<xsl:if test="list/@member='class' or array/@member='class'">static</xsl:if>
							<xsl:apply-templates select="." mode="Format"/>
						</td>
						<td>
							<img src="{$IconsFolder}Right.GIF"/>
						</td>
						<td align="center">[<xsl:value-of select="parent::relationship/@action"/>]<br/>
							<xsl:value-of select="parent::relationship/@type"/>
						</td>
						<td>
							<xsl:if test="preceding-sibling::father/@range!='no'">
								<img src="{$IconsFolder}Left.GIF"/>
							</xsl:if>
							<xsl:if test="preceding-sibling::father/@range='no'">
								<img src="{$IconsFolder}Horizontal.GIF"/>
							</xsl:if>
						</td>
						<td align="right">
							<xsl:apply-templates select="preceding-sibling::father/@cardinal"/>
							<br/>
							<xsl:value-of select="preceding-sibling::father/@range"/>
							<br/>
							<xsl:apply-templates select="preceding-sibling::father" mode="Format"/>
						</td>
					</tr>
				</table>
			</td>
			<td align="center">
				<xsl:value-of select="preceding-sibling::father/@name"/>
				<br/>
				<xsl:apply-templates select="preceding-sibling::father" mode="SimpleClass"/>
			</td>
		</tr>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="array | list" mode="Relationship">
		<xsl:value-of select="name()"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="father | child" mode="SimpleClass">
		<table class="border">
			<tr class="small">
				<td>
					<xsl:apply-templates select="id(@idref)" mode="FullPathName"/>
				</td>
			</tr>
		</table>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="class | reference" mode="SimpleClass">
		<table class="border">
			<tr class="small">
				<td>
					<xsl:apply-templates select="." mode="FullPathName"/>
				</td>
			</tr>
		</table>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="father | child" mode="Format">
		<xsl:choose>
			<xsl:when test="list and @cardinal!='1' and @cardinal!='01'">list <xsl:if test="@level='1'">of pointer</xsl:if></xsl:when>
			<xsl:when test="array and @cardinal!='1' and @cardinal!='01'">array <xsl:if test="@level='1'">of pointer</xsl:if></xsl:when>
			<xsl:when test="@level='1'">pointer</xsl:when>
			<xsl:when test="@level='0'">value</xsl:when>
			<xsl:otherwise>val\ref</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="class" mode="Class">
		<xsl:variable name="CurrentID" select="@id"/>
		<p class="comment">
			<xsl:value-of select="comment/@brief"/>
		</p>
		<table border="0">
			<tr>
				<xsl:apply-templates select="inherited"/>
			</tr>
			<tr>
				<xsl:apply-templates select="inherited" mode="Arrow"/>
			</tr>
			<tr>
			</tr>
		</table>
		<table border="0">
			<tr>
				<td>
					<table class="code">
						<tr>
							<td class="member" align="center">
								<xsl:call-template name="Formater">
									<xsl:with-param name="Text">
										<xsl:if test="@implementation!='simple'">&lt;<xsl:value-of select="@implementation"/>&gt;</xsl:if>
										<xsl:value-of select="@name"/>
									</xsl:with-param>
									<xsl:with-param name="Police">
										<xsl:choose>
											<xsl:when test="@implementation='abstract'">i</xsl:when>
											<xsl:otherwise>b</xsl:otherwise>
										</xsl:choose>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:if test="@implementation='container'">
									<span class="template">
									  <xsl:apply-templates select="model">
									    <xsl:sort select="@name"/>
                                      </xsl:apply-templates>
									</span>
								</xsl:if>
								<br/>
							</td>
						</tr>
						<tr>
							<td class="member">
								<xsl:apply-templates select="property" mode="Class"/>
							</td>
						</tr>
						<tr>
							<td class="member">
								<xsl:if test="@constructor='public'">
									<xsl:apply-templates select="@constructor"/>
									<b>Default constructor</b>
									<br/>
								</xsl:if>
								<xsl:if test="@destructor='public'">
									<xsl:apply-templates select="@destructor"/>
									<b>Destructor</b>
									<br/>
								</xsl:if>
								<xsl:apply-templates select="method[@constructor='public']" mode="Class"/>
								<xsl:apply-templates select="method[return/variable/@range='public']" mode="Class"/>
								<xsl:if test="@constructor='protected'">
									<xsl:apply-templates select="@constructor"/>
									<b>Default constructor</b>
									<br/>
								</xsl:if>
								<xsl:if test="@destructor='protected'">
									<xsl:apply-templates select="@destructor"/>
									<b>Destructor</b>
									<br/>
								</xsl:if>
								<xsl:apply-templates select="method[@constructor='protected']" mode="Class"/>
								<xsl:apply-templates select="method[return/variable/@range='protected']" mode="Class"/>
								<xsl:if test="@constructor='private'">
									<xsl:apply-templates select="@constructor"/>
									<b>Default constructor</b>
									<br/>
								</xsl:if>
								<xsl:if test="@destructor='private'">
									<xsl:apply-templates select="@destructor"/>
									<b>Destructor</b>
									<br/>
								</xsl:if>
								<xsl:apply-templates select="method[@constructor='private']" mode="Class"/>
								<xsl:apply-templates select="method[return/variable/@range='private']" mode="Class"/>
							</td>
						</tr>
					</table>
				</td>
				<td>
					<table>
						<!-- preceding-sibling::father/@range!='no' and  -->
						<xsl:apply-templates select="//relationship/child[@idref=$CurrentID]" mode="Class"/>
						<xsl:apply-templates select="//relationship/father[@idref=$CurrentID]" mode="Class"/>
						<xsl:apply-templates select="dependency" mode="Dependencies"/>
						<xsl:apply-templates select="//dependency[@idref=$CurrentID]" mode="ReverseDependencies"/>
					</table>
				</td>
			</tr>
		</table>
		<table border="0">
			<tr>
				<xsl:apply-templates select="//inherited[@idref=$CurrentID]" mode="Arrow"/>
			</tr>
			<tr>
				<xsl:apply-templates select="//inherited[@idref=$CurrentID]" mode="Child"/>
			</tr>
			<tr>
			</tr>
		</table>
		<p class="comment">
			<xsl:value-of select="comment"/>
		</p>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="model">
	  <xsl:if test="position()=1">[</xsl:if>
		<xsl:value-of select="@name"/>
	  <xsl:if test="position()!=last()">,</xsl:if>
	  <xsl:if test="position()=last()">]</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="model" mode="FullPathName">
	  <span class="template">[Class <xsl:value-of select="@name"/>]</span>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="property" mode="Class">
        <xsl:variable name="ClassImpl" select="parent::class/@implementation"/>
		<xsl:call-template name="Formater">
			<xsl:with-param name="Text">
				<xsl:choose>
                  <xsl:when test="$ClassImpl='abstract'">+&lt;abstract&gt;</xsl:when>
                  <xsl:when test="@overridable='yes'">
                    <xsl:apply-templates select="variable/@range"/>&lt;overridable&gt;</xsl:when>
                    <xsl:otherwise><xsl:apply-templates select="variable/@range"/></xsl:otherwise>
                </xsl:choose>
				<xsl:value-of select="@name"/>
				<xsl:if test="variable/@valref">=<xsl:value-of select="id(variable/@valref)/@name"/></xsl:if>
				<xsl:if test="variable/@value">=<xsl:value-of select="variable/@value"/></xsl:if>
			</xsl:with-param>
			<xsl:with-param name="Police">
				<xsl:choose>
					<xsl:when test="$ClassImpl='abstract'">i</xsl:when>
					<xsl:when test="@member='class'">u</xsl:when>
					<xsl:otherwise>br</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
		</xsl:call-template>
		<br/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="method" mode="Class">
		<xsl:call-template name="Formater">
			<xsl:with-param name="Text">
				<xsl:apply-templates select="return/variable/@range"/>
				<xsl:if test="@implementation!='simple'">
					<xsl:apply-templates select="@implementation"/>
				</xsl:if>
				<xsl:if test="@constructor!='no'">
					<xsl:apply-templates select="@constructor"/>Constructor</xsl:if>
				<xsl:if test="@constructor='no'">
					<xsl:apply-templates select="@constructor"/>
					<xsl:value-of select="@name"/>
				</xsl:if>(<xsl:apply-templates select="param" mode="Class"/>)</xsl:with-param>
			<xsl:with-param name="Police">
				<xsl:choose>
					<xsl:when test="@constructor!='no'">b</xsl:when>
					<xsl:when test="@implementation='abstract'">i</xsl:when>
					<xsl:when test="@member='class'">u</xsl:when>
					<xsl:otherwise>br</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
		</xsl:call-template>
		<br/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@implementation">
		<xsl:value-of select="concat('&lt;',.,'&gt;')"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@range | @constructor | @destructor">
		<xsl:choose>
			<xsl:when test=".='public'">+</xsl:when>
			<xsl:when test=".='protected'">#</xsl:when>
			<xsl:when test=".='private'">-</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@member">
		<xsl:if test=".='class'">&lt;class&gt;</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@inline">
		<xsl:if test=".='yes'">&lt;inline&gt;</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="param" mode="Class">
		<xsl:value-of select="@name"/>
		<xsl:if test="variable/@valref">=<xsl:value-of select="id(variable/@valref)/@name"/></xsl:if>
		<xsl:if test="variable/@value">=<xsl:value-of select="variable/@value"/></xsl:if>
		<xsl:if test="position()&lt;last()">,</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Formater">
		<xsl:param name="Text"/>
		<xsl:param name="Police"/>
		<!--xsl:value-of select="concat($Text,'(',$Police,')')"/-->
		<xsl:if test="$Police!='br'">
			<xsl:element name="{$Police}">
				<xsl:value-of select="$Text"/>
			</xsl:element>
		</xsl:if>
		<xsl:if test="$Police='br'">
			<xsl:value-of select="$Text"/>
		</xsl:if>
		<!--br/-->
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="typedef" mode="FullPathName">
		<span class="class-package">
			<xsl:apply-templates select="parent::class" mode="FullPathName"/>
		</span><xsl:value-of select="@name"/></xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="reference" mode="FullPathName">
		<span class="package">
    		<xsl:apply-templates select="ancestor::import" mode="FullPathName"/>
			<xsl:if test="@package"><xsl:value-of select="@package"/><xsl:value-of select="$Separator"/></xsl:if>
		</span><xsl:value-of select="@name"/></xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="class" mode="FullPathName">
		<span class="package">
			<xsl:apply-templates select="parent::package" mode="FullPathName"/>
		</span><xsl:value-of select="@name"/></xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="import" mode="FullPathName">
        <xsl:if test="@param"><xsl:value-of select="@param"/><xsl:value-of select="$Separator"/></xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="package" mode="FullPathName">
		<xsl:apply-templates select="parent::package" mode="FullPathName"/>
		<xsl:value-of select="@name"/><xsl:value-of select="$Separator"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@cardinal">
		<xsl:choose>
			<xsl:when test=".='0n'">0..n</xsl:when>
			<xsl:when test=".='1n'">1..n</xsl:when>
			<xsl:when test=".='01'">0..1</xsl:when>
			<xsl:when test=".='01'">1</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="typedef | property | param | return | get | set | element" mode="Typedef">
		<xsl:variable name="NodeName" select="name()"/>
		<table border="0">
			<tr valign="top">
				<xsl:if test="$NodeName='get' or $NodeName='set'">
					<td/>
					<td>
						<xsl:apply-templates select="@range"/>
						<xsl:value-of select="$NodeName"/> accessor</td>
				</xsl:if>
				<xsl:if test="$NodeName='property'">
					<td>
        				<xsl:choose>
                          <xsl:when test="parent::class/@implementation='abstract'">+&lt;abstract&gt;</xsl:when>
                          <xsl:when test="@overridable='yes'">
                            <xsl:apply-templates select="variable/@range"/>&lt;overridable&gt;</xsl:when>
                            <xsl:otherwise><xsl:apply-templates select="variable/@range"/></xsl:otherwise>
                        </xsl:choose>
            <p/>Attribute: <xsl:value-of select="@attribute"/>
					</td>
                </xsl:if>
				<xsl:if test="$NodeName!='param' and $NodeName!='property'">
					<td>
						<xsl:apply-templates select="variable/@range[$NodeName!='return']"/>
					</td>
				</xsl:if>
				<xsl:if test="$NodeName='param' and(variable/@value or variable/@valref)">
					<td>optional</td>
				</xsl:if>
				<td>
					<xsl:apply-templates select="@member[$NodeName!='return']"/>
					<xsl:apply-templates select="parent::method/@inline[$NodeName='return']"/>
				</td>
				<td>
					<xsl:apply-templates select="variable/@modifier | @modifier"/>
				</td>
				<td>
					<xsl:apply-templates select="variable/@level | @level"/>
				</td>
				<td>
					<xsl:value-of select="@name"/>
					<xsl:apply-templates select="parent::method[$NodeName='return']" mode="Class"/>
				</td>
				<xsl:apply-templates select="variable/@size | variable/@sizeref"/>
				<td><xsl:if test="not(self::get) and not(self::set)">:</xsl:if></td>
				<xsl:if test="not(type/@struct)">
					<td>
						<xsl:apply-templates select="type/@modifier"/>
					</td>
					<!--td>
						<xsl:apply-templates select="type/@level"/>
					</td-->
				</xsl:if>
				<td>
					<xsl:apply-templates select="type"/>
					<xsl:apply-templates select="self::element/@desc"/>
					<xsl:apply-templates select="id(self::element/@idref)" mode="FullPathName"/>
				</td>
				<td>
					<xsl:apply-templates select="type/@by | @by"/>
				</td>
				<xsl:apply-templates select="variable/@value | variable/@valref"/>
				<td class="comment">
					<xsl:value-of select="comment/text()"/>
					<xsl:value-of select="text()"/>
				</td>
				<xsl:if test="$NodeName='property'">
					<xsl:apply-templates select="get[@range!='no'] | set[@range!='no']" mode="Typedef"/>
				</xsl:if>
			</tr>
		</table>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@by">
		<xsl:if test=".='ref'">&lt;byref&gt;</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@value">
		<td>=<xsl:value-of select="."/></td>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@valref">
		<td>=<xsl:value-of select="id(.)/@name"/></td>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@size">
		<td>[<xsl:value-of select="."/>]</td>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@sizeref">
		<td>[<xsl:value-of select="id(.)/@name"/>]</td>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@modifier">
		<xsl:if test=".='const'">&lt;const&gt;</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@level">
		<xsl:if test="number(.)=1">&lt;pointer&gt;</xsl:if>
		<xsl:if test="number(.)=2">&lt;handler&gt;</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="type">
		<xsl:if test="@struct">
			<xsl:value-of select="concat('&lt;',@struct,'&gt; ')"/>
		</xsl:if>
		<xsl:if test="list">
			<xsl:apply-templates select="list/@desc"/>
			<xsl:apply-templates select="id(list/@idref)" mode="FullPathName"/>
			<xsl:text> (</xsl:text>
			<xsl:if test="list/@type='indexed'">
				<xsl:apply-templates select="list/@level"/>
				<xsl:text> </xsl:text>
				<xsl:value-of select="list/@index-desc"/>
				<xsl:apply-templates select="id(list/@index-idref)" mode="FullPathName"/>
				<xsl:text>, </xsl:text>
			</xsl:if>
		</xsl:if>
		<xsl:apply-templates select="@level"/> 
		<xsl:text> </xsl:text>
		<xsl:value-of select="@desc"/>
		<xsl:apply-templates select="id(@idref)" mode="FullPathName"/>
		<xsl:apply-templates select="enumvalue"/>
		<xsl:apply-templates select="element" mode="List"/>
		<xsl:apply-templates select="list"/>
		<xsl:apply-templates select="array"/>
		<xsl:if test="list">)</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="exception" mode="Exception">
		<xsl:apply-templates select="." mode="Method"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="exception" mode="Method">
		<li>
			<xsl:value-of select="id(@idref)/@name"/>
		</li>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="enumvalue">
		<xsl:if test="position()=1">{<br/></xsl:if>
		<xsl:value-of select="@name"/>
		<xsl:if test="@value">=<xsl:value-of select="@value"/></xsl:if>
		<span class="comment">: <xsl:value-of select="."/></span>
		<br/>
		<xsl:if test="position()=last()">}</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="element" mode="List">
		<xsl:if test="position()=1">{<br/></xsl:if>
		<xsl:apply-templates select="." mode="Typedef"/>
		<xsl:if test="position()=last()">}</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="method" mode="Name">
		<xsl:if test="@name">
			<xsl:value-of select="@name"/>
		</xsl:if>
		<xsl:if test="not(@name)">&lt;Constructor&gt;</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="ExternalPackageView">
	<xsl:for-each select="//import">
      <table border="0">
        <tr>
          <td><img src="{$IconsFolder}HorizD.GIF"/></td>
          <td><img src="{$IconsFolder}LeftD.GIF"/></td>
          <td><xsl:apply-templates select="." mode="PackageImg"/></td>
        </tr>
      </table>
    </xsl:for-each>
    </xsl:template>
	<!-- ======================================================================= -->
    <xsl:template name="PackageView">
    <xsl:variable name="Request">
    <xsl:comment>class//type</xsl:comment>
    <xsl:for-each select="class//type">
      <xsl:choose>
        <xsl:when test="@idref">
          <xsl:apply-templates select="id(@idref)" mode="PackageView"/>
        </xsl:when>
        <xsl:when test="id(list/@idref)/self::reference">
          <xsl:apply-templates select="id(list/@idref)" mode="PackageView"/>
        </xsl:when>
        <xsl:when test="id(list/@index-idref)/self::reference">
          <xsl:apply-templates select="id(list/@index-idref)" mode="PackageView"/>
        </xsl:when>
      </xsl:choose>
    </xsl:for-each>
    <xsl:comment>class/inherited</xsl:comment>
    <xsl:for-each select="class/inherited">
      <xsl:apply-templates select="id(@idref)" mode="PackageView"/>
    </xsl:for-each>
    <xsl:comment>class/dependency</xsl:comment>
    <xsl:for-each select="class/dependency">
      <xsl:apply-templates select="id(@idref)" mode="PackageView"/>
    </xsl:for-each>
    <xsl:comment>class/collaboration</xsl:comment>
    <xsl:for-each select="class[collaboration]">
      <xsl:variable name="ClassID" select="@id"/>
      <xsl:apply-templates select="id(collaboration/@idref)/*[@idref!=$ClassID]" mode="PackageView"/>
    </xsl:for-each>
    </xsl:variable>
    <xsl:variable name="Name" select="@name"/>
    <xsl:for-each select="msxsl:node-set($Request)//reference[generate-id()=generate-id(key('package',@package)[1])]">
      <xsl:apply-templates select="self::*[@package!=$Name]" mode="PackageImg"/>
    </xsl:for-each>
    </xsl:template>
	<!-- ======================================================================= -->
    <xsl:template match="father | child" mode="PackageView">
      <xsl:apply-templates select="id(@idref)" mode="PackageView"/>
    </xsl:template>
	<!-- ======================================================================= -->
    <xsl:template match="reference" mode="PackageView">
         <xsl:element name="reference">
           <xsl:copy-of select="@name"/>
          <xsl:attribute name="package">
            <xsl:choose>
              <xsl:when test="string-length(ancestor::import/@param)!=0"><xsl:value-of select="ancestor::import/@param"/></xsl:when>
              <xsl:otherwise><xsl:value-of select="ancestor::import/@name"/></xsl:otherwise>
            </xsl:choose>
            <xsl:if test="@package"><xsl:value-of select="$Separator"/><xsl:value-of select="@package"/></xsl:if>
          </xsl:attribute>
        </xsl:element><xsl:text>
</xsl:text>
    </xsl:template>
	<!-- ======================================================================= -->
    <xsl:template match="class" mode="PackageView">
         <xsl:element name="reference">
           <xsl:copy-of select="@name"/>
          <xsl:attribute name="package">
            <xsl:value-of select="parent::*/@name"/>
          </xsl:attribute>
        </xsl:element><xsl:text>
</xsl:text>
    </xsl:template>
	<!-- ======================================================================= -->
    <xsl:template match="typedef" mode="PackageView">
         <xsl:element name="reference">
           <xsl:copy-of select="@name"/>
          <xsl:attribute name="package">
            <xsl:value-of select="parent::class/parent::*/@name"/>
          </xsl:attribute>
        </xsl:element><xsl:text>
</xsl:text>
    </xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="reference" mode="PackageImg">
      <table border="0">
        <tr>
          <td><img src="{$IconsFolder}HorizD.GIF"/></td>
          <td><img src="{$IconsFolder}LeftD.GIF"/></td>
          <td>
          	  <table border="0">
                <tr><td><img src="{$IconsFolder}Package.gif"/></td></tr>
                <tr><td align="center"><xsl:value-of select="@package"/></td></tr>
              </table>
          </td>
        </tr>
      </table>
    </xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="*" mode="PackageImg">
  	  <table border="0">
        <tr><td><img src="{$IconsFolder}Package.gif"/></td></tr>
        <tr><td align="center">
          <xsl:choose>
            <xsl:when test="string-length(@param)!=0"><xsl:value-of select="@param"/></xsl:when>
            <xsl:otherwise><xsl:value-of select="@name"/></xsl:otherwise>
          </xsl:choose>
        </td></tr>
      </table>
    </xsl:template>
	<!-- ======================================================================= -->
</xsl:stylesheet>








