<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:uml="http://schema.omg.org/spec/UML/2.1" xmlns:xmi="http://schema.omg.org/spec/XMI/2.1">
	<!-- ======================================================================= -->
	<!--xsl:output indent="yes" method="xml" encoding="utf-8" media-type="xprj"/-->
	<xsl:output indent="yes" method="xml" encoding="UTF-8" media-type="xprj" standalone="no" doctype-system="class-model.dtd"/>
	<!-- ============================================================================== -->
	<xsl:attribute-set name="Type">
		<xsl:attribute name="level">0</xsl:attribute>
		<xsl:attribute name="by">val</xsl:attribute>
		<xsl:attribute name="modifier">var</xsl:attribute>
	</xsl:attribute-set>
	<!--  ============================================================================== -->
	<xsl:attribute-set name="Class">
		<xsl:attribute name="name">Unknown</xsl:attribute>
		<xsl:attribute name="visibility">common</xsl:attribute>
		<xsl:attribute name="constructor">no</xsl:attribute>
		<xsl:attribute name="destructor">no</xsl:attribute>
		<xsl:attribute name="inline">none</xsl:attribute>
		<xsl:attribute name="implementation">abstract</xsl:attribute>
	</xsl:attribute-set>
	<!--  ============================================================================== -->
	<xsl:attribute-set name="Element">
		<xsl:attribute name="name">
			<xsl:value-of select="@name"/>
			<xsl:if test="not(@name) or @name=''">
				<xsl:value-of select="generate-id()"/>
			</xsl:if>
		</xsl:attribute>
		<xsl:attribute name="num-id">
			<xsl:value-of select="position()"/>
		</xsl:attribute>
		<xsl:attribute name="level">0</xsl:attribute>
		<xsl:attribute name="modifier">var</xsl:attribute>
	</xsl:attribute-set>
	<!--  ============================================================================== -->
	<xsl:attribute-set name="Node">
		<xsl:attribute name="name">
			<xsl:apply-templates select="@name"/>
			<xsl:if test="not(@name) or @name=''">
				<xsl:value-of select="generate-id()"/>
			</xsl:if>
		</xsl:attribute>
		<xsl:attribute name="id">
			<xsl:value-of select="@xmi:id"/>
		</xsl:attribute>
	</xsl:attribute-set>
	<!-- ============================================================================== -->
	<xsl:template match="@name">
		<xsl:value-of select="translate(.,'- ,;/\','________')"/>
	</xsl:template>
	<!-- ============================================================================== -->
	<xsl:template match="/*">
		<xsl:if test="@xmi:version!='2.1'">
			<xsl:message terminate="yes">Expected XMI/UML 2.1 !</xsl:message>
		</xsl:if>
		<xsl:element name="root">
			<!-- UML Designer DTD version -->
			<xsl:attribute name="version">1.3</xsl:attribute>
			<xsl:attribute name="name">
				<xsl:value-of select="*[name()='uml:Model']/@name"/>
			</xsl:attribute>
			<xsl:element name="generation">
				<xsl:attribute name="destination">C:\</xsl:attribute>
				<xsl:attribute name="language">0</xsl:attribute>
			</xsl:element>
			<xsl:element name="comment">
				<xsl:variable name="ID" select="*[name()='uml:Model']/@xmi:id"/>
				<xsl:attribute name="brief">
					<xsl:value-of select="//*[@base_Package=$ID]/@description"/>
				</xsl:attribute>
				<xsl:text>A detailed comment</xsl:text>
			</xsl:element>
			<xsl:if test="//nestedClassifier/nestedClassifier">
				<xsl:element name="class" use-attribute-sets="Class">
					<xsl:attribute name="id">
						<xsl:value-of select="concat(generate-id(),'_Unknown')"/>
					</xsl:attribute>
					<xsl:element name="comment">
						<xsl:attribute name="brief">Unknown classes</xsl:attribute>
					</xsl:element>
					<xsl:apply-templates select="//nestedClassifier/nestedClassifier"/>
				</xsl:element>
			</xsl:if>
			<xsl:apply-templates select="*[name()='uml:Model']/packagedElement[@xmi:type!='uml:Package' and @xmi:type!='uml:Association']"/>
			<xsl:apply-templates select="*[name()='uml:Model']/packagedElement[@xmi:type='uml:Package']"/>
			<xsl:apply-templates select="//packagedElement[@xmi:type='uml:Association']"/>
			<xsl:apply-templates select="//packagedElement[@xmi:type='uml:AssociationClass']" mode="AssociationClass"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="packagedElement">
		<xsl:choose>
			<xsl:when test="@xmi:type='uml:Package'">
				<xsl:if test="descendant::packagedElement[@xmi:type='uml:Class' or @xmi:type='uml:Interface' or @xmi:type='uml:Enumeration']">
					<xsl:call-template name="Package"/>
				</xsl:if>
			</xsl:when>
			<xsl:when test="@xmi:type='uml:Class'">
				<xsl:call-template name="Class"/>
			</xsl:when>
			<xsl:when test="@xmi:type='uml:AssociationClass'">
				<xsl:call-template name="Class"/>
			</xsl:when>
			<xsl:when test="@xmi:type='uml:Interface'">
				<xsl:call-template name="Class"/>
			</xsl:when>
			<xsl:when test="@xmi:type='uml:Association'">
				<xsl:call-template name="Association"/>
			</xsl:when>
			<xsl:when test="@xmi:type='uml:Enumeration'">
				<xsl:call-template name="Unknown"/>
			</xsl:when>
			<xsl:otherwise>
				<!--xsl:call-template name="Unknown"/-->
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="nestedClassifier">
		<xsl:variable name="Name" select="@name"/>
		<xsl:if test="not(parent::*/descendant::ownedParameteredElement[@name=$Name])">
			<xsl:element name="typedef" use-attribute-sets="Node">
				<xsl:element name="type">
					<xsl:attribute name="level">0</xsl:attribute>
					<xsl:attribute name="by">val</xsl:attribute>
					<xsl:attribute name="modifier">var</xsl:attribute>
					<xsl:choose>
						<xsl:when test="not(ownedAttribute) and not(ownedLiteral)">
							<xsl:attribute name="desc">undef_typedef</xsl:attribute>
						</xsl:when>
						<xsl:when test="ownedAttribute">
							<xsl:attribute name="struct">struct</xsl:attribute>
							<xsl:apply-templates select="ownedAttribute" mode="Element"/>
						</xsl:when>
						<xsl:when test="ownedLiteral">
							<xsl:apply-templates select="ownedLiteral" mode="Enumeration"/>
						</xsl:when>
					</xsl:choose>
				</xsl:element>
				<xsl:element name="variable">
					<xsl:attribute name="range">
						<xsl:choose>
							<xsl:when test="@visibility='package'">public</xsl:when>
							<xsl:otherwise>private</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</xsl:element>
				<xsl:apply-templates select="." mode="CommentDataType"/>
			</xsl:element>
		</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedLiteral">
		<xsl:element name="property">
			<xsl:attribute name="name">
				<xsl:value-of select="@name"/>
				<xsl:if test="not(@name) or @name=''">
					<xsl:value-of select="generate-id()"/>
				</xsl:if>
			</xsl:attribute>
			<xsl:attribute name="num-id">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="member">class</xsl:attribute>
			<xsl:element name="type">
				<xsl:attribute name="level">0</xsl:attribute>
				<xsl:attribute name="by">val</xsl:attribute>
				<xsl:attribute name="modifier">const</xsl:attribute>

				<xsl:attribute name="desc">int16</xsl:attribute>
			</xsl:element>
			<xsl:element name="variable">
				<xsl:attribute name="range">public</xsl:attribute>
				<xsl:attribute name="value">
					<xsl:value-of select="position()-1"/>
				</xsl:attribute>
			</xsl:element>
			<xsl:element name="comment">
				<xsl:apply-templates select="." mode="Comment"/>
			</xsl:element>
			<xsl:element name="get">
				<xsl:attribute name="by">val</xsl:attribute>
				<xsl:attribute name="modifier">var</xsl:attribute>
				<xsl:attribute name="range">no</xsl:attribute>
			</xsl:element>
			<xsl:element name="set">
				<xsl:attribute name="by">val</xsl:attribute>
				<xsl:attribute name="range">no</xsl:attribute>
			</xsl:element>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedLiteral" mode="Enumeration">
		<xsl:element name="enumvalue">
			<xsl:attribute name="id">
				<xsl:value-of select="@xmi:id"/>
			</xsl:attribute>
			<xsl:attribute name="name">
				<xsl:value-of select="@name"/>
				<xsl:if test="not(@name) or @name=''">
					<xsl:value-of select="generate-id()"/>
				</xsl:if>
			</xsl:attribute>
			<xsl:if test="specification[string(number(@value))!='NaN']">
				<xsl:attribute name="value">
					<xsl:value-of select="specification/@value"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="." mode="Comment"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedAttribute" mode="Element">
		<xsl:element name="element" use-attribute-sets="Element">
			<xsl:choose>
				<xsl:when test="@type">
					<xsl:apply-templates select="@type" mode="Description"/>
				</xsl:when>
				<xsl:when test="type">
					<xsl:apply-templates select="type/@xmi:idref" mode="Description"/>
				</xsl:when>
			</xsl:choose>
			<xsl:apply-templates select="." mode="ElementComment"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedAttribute">
		<xsl:choose>
			<xsl:when test="not(@association)">
				<xsl:call-template name="Property"/>
			</xsl:when>
			<xsl:otherwise/>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedOperation">
		<xsl:choose>
			<xsl:when test="@xmi:type='uml:Operation'">
				<xsl:call-template name="Method"/>
			</xsl:when>
			<xsl:otherwise/>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedOperation" mode="Constructor">
		<xsl:attribute name="constructor">
			<xsl:value-of select="@visibility"/>
		</xsl:attribute>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedOperation" mode="Destructor">
		<xsl:attribute name="destructor">
			<xsl:value-of select="@visibility"/>
		</xsl:attribute>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedParameter">
		<xsl:choose>
			<xsl:when test="@direction='return'">
				<xsl:call-template name="Return"/>
			</xsl:when>
			<xsl:when test="@name!=''">
				<xsl:call-template name="Parameter"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@type">
		<xsl:variable name="ID" select="."/>
		<xsl:choose>
			<xsl:when test="//packagedElement[(@xmi:type='uml:PrimitiveType' or @xmi:type='uml:DataType') and @xmi:id=$ID]">
				<xsl:attribute name="desc">
					<xsl:value-of select="//packagedElement[@xmi:id=$ID]/@name"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:when test="//*[@xmi:id=$ID]">
				<xsl:attribute name="idref">
					<xsl:value-of select="$ID"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="idref">INCONNU</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="type">
		<xsl:variable name="ID" select="@xmi:idref"/>
		<xsl:choose>
			<xsl:when test="//packagedElement[(@xmi:type='uml:PrimitiveType' or @xmi:type='uml:DataType') and @xmi:id=$ID]">
				<xsl:attribute name="desc">
					<xsl:value-of select="//packagedElement[@xmi:id=$ID]/@name"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:when test="//*[@xmi:id=$ID]">
				<xsl:attribute name="idref">
					<xsl:value-of select="$ID"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="idref">INCONNU</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Package">
		<xsl:element name="package" use-attribute-sets="Node">
			<xsl:apply-templates select="." mode="Comment"/>
			<xsl:apply-templates select="packagedElement[@xmi:type!='uml:Package' and @xmi:type!='uml:Association']"/>
			<xsl:apply-templates select="packagedElement[@xmi:type='uml:Package']"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="AssociationClass" match="packagedElement" mode="AssociationClass">
		<xsl:element name="relationship">
			<xsl:variable name="ID" select="@xmi:id"/>
			<xsl:attribute name="id">
				<xsl:value-of select="generate-id()"/>
			</xsl:attribute>
			<xsl:attribute name="action">
				<xsl:value-of select="ownedComment/@body"/>
				<xsl:value-of select="ownedComment/body"/>
				<xsl:if test="not(ownedComment)">
					<xsl:apply-templates select="//ownedAttribute[@association=$ID][1]" mode="Action"/>
				</xsl:if>
			</xsl:attribute>
			<xsl:attribute name="type">
				<xsl:choose>
					<xsl:when test="not(memberEnd) and not(ownedEnd)">
						<xsl:apply-templates select="@memberEnd" mode="TypeAssociation"/>
					</xsl:when>
					<xsl:when test="*[@aggregation]">
						<xsl:apply-templates select="*[@aggregation][1]" mode="TypeAssociation"/>
					</xsl:when>
					<xsl:otherwise>assembl</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:apply-templates select="ownedEnd[1]" mode="ParentAssociation">
				<xsl:with-param name="Association" select="generate-id()"/>
				<xsl:with-param name="Navigable">yes</xsl:with-param>
				<xsl:with-param name="ChildName">father</xsl:with-param>
				<xsl:with-param name="Index">
					<xsl:value-of select="$ID"/>
				</xsl:with-param>
			</xsl:apply-templates>
			<xsl:apply-templates select="ownedEnd[2]" mode="ParentAssociation">
				<xsl:with-param name="Association" select="generate-id()"/>
				<xsl:with-param name="Navigable">yes</xsl:with-param>
				<xsl:with-param name="ChildName">child</xsl:with-param>
				<xsl:with-param name="Index">
					<xsl:value-of select="$ID"/>
				</xsl:with-param>
			</xsl:apply-templates>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Association">
		<xsl:element name="relationship">
			<xsl:variable name="ID" select="@xmi:id"/>
			<xsl:attribute name="id">
				<xsl:value-of select="$ID"/>
			</xsl:attribute>
			<xsl:attribute name="action">
				<xsl:value-of select="ownedComment/@body"/>
				<xsl:value-of select="ownedComment/body"/>
				<xsl:if test="not(ownedComment)">
					<xsl:apply-templates select="//ownedAttribute[@association=$ID][1]" mode="Action"/>
				</xsl:if>
			</xsl:attribute>
			<xsl:attribute name="type">
				<xsl:choose>
					<xsl:when test="not(memberEnd) and not(ownedEnd)">
						<xsl:apply-templates select="@memberEnd" mode="TypeAssociation"/>
					</xsl:when>
					<xsl:when test="*[@aggregation]">
						<xsl:apply-templates select="*[@aggregation][1]" mode="TypeAssociation"/>
					</xsl:when>
					<xsl:otherwise>assembl</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="not(ownedEnd)">
					<xsl:for-each select="//ownedAttribute[@association=$ID]">
						<xsl:apply-templates select="." mode="Association">
							<xsl:with-param name="ChildName">
								<xsl:choose>
									<xsl:when test="position()=1">father</xsl:when>
									<xsl:otherwise>child</xsl:otherwise>
								</xsl:choose>
							</xsl:with-param>
						</xsl:apply-templates>
					</xsl:for-each>
				</xsl:when>
				<xsl:when test="ownedEnd[1] and ownedEnd[2]">
					<xsl:apply-templates select="ownedEnd[1]" mode="ParentAssociation">
						<xsl:with-param name="Association" select="$ID"/>
						<xsl:with-param name="Navigable">no</xsl:with-param>
						<xsl:with-param name="ChildName">father</xsl:with-param>
					</xsl:apply-templates>
					<xsl:apply-templates select="ownedEnd[2]" mode="ParentAssociation">
						<xsl:with-param name="Association" select="$ID"/>
						<xsl:with-param name="Navigable">yes</xsl:with-param>
						<xsl:with-param name="ChildName">child</xsl:with-param>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="ownedEnd" mode="ParentAssociation">
						<xsl:with-param name="Association" select="$ID"/>
						<xsl:with-param name="Navigable">no</xsl:with-param>
						<xsl:with-param name="ChildName">father</xsl:with-param>
					</xsl:apply-templates>
					<xsl:apply-templates select="//ownedAttribute[@association=$ID]" mode="Association">
						<xsl:with-param name="ChildName">child</xsl:with-param>
					</xsl:apply-templates>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@memberEnd" mode="TypeAssociation">
		<xsl:variable name="IDs" select="."/>
		<xsl:apply-templates select="//*[@xmi:id=substring-before($IDs,' ')]" mode="TypeAssociation"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="memberEnd | ownedEnd | ownedAttribute" mode="TypeAssociation">
		<xsl:choose>
			<xsl:when test="@aggregation='shared'">aggreg</xsl:when>
			<xsl:when test="@aggregation='composite'">comp</xsl:when>
			<xsl:otherwise>assembl</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedEnd" mode="ParentAssociation">
		<xsl:param name="Association"/>
		<xsl:param name="Navigable"/>
		<xsl:param name="ChildName"/>
		<xsl:param name="Index"/>
		<xsl:choose>
			<xsl:when test="$Navigable='no'">
				<xsl:call-template name="NotNavigable">
					<xsl:with-param name="ChildName">father</xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="Navigable">
					<xsl:with-param name="ChildName" select="$ChildName"/>
					<xsl:with-param name="Index" select="$Index"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="NotNavigable">
		<xsl:param name="ChildName"/>
		<xsl:apply-templates select="." mode="Association">
			<xsl:with-param name="ChildName" select="$ChildName"/>
		</xsl:apply-templates>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Navigable">
		<xsl:param name="ChildName"/>
		<xsl:param name="Index"/>
		<xsl:variable name="ID" select="parent::*/@xmi:id"/>
		<xsl:choose>
			<xsl:when test="//ownedAttribute[$ID=@association]">
				<xsl:apply-templates select="//ownedAttribute[$ID=@association]" mode="Association">
					<xsl:with-param name="ChildName" select="$ChildName"/>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="." mode="Association">
					<xsl:with-param name="ChildName" select="$ChildName"/>
					<xsl:with-param name="Index" select="$Index"/>
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedAttribute" mode="Comment">
		<xsl:variable name="ID" select="@xmi:id"/>
		<xsl:variable name="Comment">
			<xsl:value-of select="ownedComment/@body"/>
			<xsl:value-of select="ownedComment/body"/>
			<xsl:value-of select="//*[@base_Property=$ID]/@description"/>
		</xsl:variable>
		<xsl:element name="comment">
			<xsl:choose>
				<xsl:when test="$Comment!=''">
					<xsl:value-of select="$Comment"/>
				</xsl:when>
				<xsl:otherwise>Detailed comment</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedAttribute" mode="ElementComment">
		<xsl:variable name="ID" select="@xmi:id"/>
		<xsl:variable name="Comment">
			<xsl:value-of select="ownedComment/@body"/>
			<xsl:value-of select="ownedComment/body"/>
			<xsl:value-of select="//*[@base_Attribute=$ID]/@description"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$Comment!=''">
				<xsl:value-of select="$Comment"/>
			</xsl:when>
			<xsl:otherwise>Detailed comment</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedComment">
		<xsl:variable name="Comment">
			<xsl:value-of select="ownedComment/@body"/>
			<xsl:value-of select="ownedComment/body"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$Comment!=''">
				<xsl:value-of select="$Comment"/>
			</xsl:when>
			<xsl:otherwise>Detailed comment</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="nestedClassifier" mode="CommentDataType">
		<xsl:variable name="ID" select="@xmi:id"/>
		<xsl:variable name="Comment">
			<xsl:value-of select="ownedComment/@body"/>
			<xsl:value-of select="ownedComment/body"/>
			<xsl:value-of select="//*[@base_DataType=$ID]/@description"/>
		</xsl:variable>
		<xsl:element name="comment">
			<xsl:choose>
				<xsl:when test="$Comment!=''">
					<xsl:value-of select="$Comment"/>
				</xsl:when>
				<xsl:otherwise>Detailed comment</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedLiteral" mode="Comment">
		<xsl:variable name="ID" select="@xmi:id"/>
		<xsl:variable name="Comment">
			<xsl:value-of select="ownedComment/@body"/>
			<xsl:value-of select="ownedComment/body"/>
			<xsl:value-of select="//*[@base_EnumerationLiteral=$ID]/@description"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$Comment!=''">
				<xsl:value-of select="$Comment"/>
			</xsl:when>
			<xsl:otherwise>Detailed comment</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedParameter" mode="Comment">
		<xsl:variable name="ID" select="@xmi:id"/>
		<xsl:variable name="Comment">
			<xsl:value-of select="ownedComment/@body"/>
			<xsl:value-of select="ownedComment/body"/>
			<xsl:value-of select="//*[@base_Parameter=$ID]/@description"/>
		</xsl:variable>
		<xsl:element name="comment">
			<xsl:choose>
				<xsl:when test="$Comment!=''">
					<xsl:value-of select="$Comment"/>
				</xsl:when>
				<xsl:otherwise>Detailed comment</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedParameter" mode="ReturnComment">
		<xsl:variable name="ID" select="@xmi:id"/>
		<xsl:variable name="Comment">
			<xsl:value-of select="ownedComment/@body"/>
			<xsl:value-of select="ownedComment/body"/>
			<xsl:value-of select="//*[@base_Parameter=$ID]/@description"/>
		</xsl:variable>
		<xsl:element name="comment">
			<xsl:choose>
				<xsl:when test="$Comment!=''">
					<xsl:value-of select="$Comment"/>
				</xsl:when>
				<xsl:otherwise>Return comment</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedAttribute" mode="Action">
		<xsl:variable name="ID" select="@xmi:id"/>
		<xsl:if test="position()=1">
			<xsl:value-of select="ownedComment/@body"/>
			<xsl:value-of select="ownedComment/body"/>
			<xsl:value-of select="//*[@base_Property=$ID]/@description"/>
		</xsl:if>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedOperation" mode="Comment">
		<xsl:element name="comment">
			<xsl:variable name="ID" select="@xmi:id"/>
			<xsl:variable name="Brief">
				<xsl:value-of select="ownedComment/@body"/>
				<xsl:value-of select="ownedComment/body"/>
				<xsl:value-of select="//*[@base_Operations=$ID]/@description"/>
			</xsl:variable>
			<xsl:attribute name="brief">
				<xsl:choose>
					<xsl:when test="$Brief!=''">
						<xsl:value-of select="$Brief"/>
					</xsl:when>
					<xsl:otherwise>Brief comment</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:call-template name="BodyComment"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="BodyComment">
		<xsl:choose>
			<xsl:when test="ownedComment/ownedComment/@body">
				<xsl:value-of select="ownedComment/ownedComment/@body"/>
			</xsl:when>
			<xsl:when test="ownedComment/body">
				<xsl:value-of select="ownedComment/body"/>
			</xsl:when>
			<xsl:otherwise>Detailed comment</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="packagedElement" mode="Comment">
		<xsl:element name="comment">
			<xsl:variable name="ID" select="@xmi:id"/>
			<xsl:variable name="Brief">
				<xsl:choose>
					<xsl:when test="@xmi:type='uml:Class' or @xmi:type='uml:Interface'">
						<xsl:value-of select="ownedComment/@body"/>
						<xsl:value-of select="ownedComment/body"/>
						<xsl:value-of select="//*[@base_Class=$ID]/@description"/>
					</xsl:when>
					<xsl:when test="@xmi:type='uml:Package'">
						<xsl:value-of select="ownedComment/@body"/>
						<xsl:value-of select="ownedComment/body"/>
						<xsl:value-of select="//*[@base_Package=$ID]/@description"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>
			<xsl:attribute name="brief">
				<xsl:choose>
					<xsl:when test="$Brief!=''">
						<xsl:value-of select="$Brief"/>
					</xsl:when>
					<xsl:otherwise>Brief comment</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:call-template name="BodyComment"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedAttribute | ownedEnd" mode="Association">
		<xsl:param name="ChildName"/>
		<xsl:param name="Index"/>
		<xsl:element name="{$ChildName}">
			<xsl:attribute name="name">
				<xsl:if test="string-length(@name)=0">UnknownMember</xsl:if>
				<xsl:value-of select="@name"/>
			</xsl:attribute>
			<xsl:attribute name="range">
				<xsl:choose>
					<xsl:when test="@visibility">
						<xsl:choose>
							<xsl:when test="@isNavigable='false'">no</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@visibility"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="$ChildName='child'">private</xsl:when>
					<xsl:otherwise>no</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="@type">
					<xsl:apply-templates select="@type" mode="Description"/>
				</xsl:when>
				<xsl:when test="type">
					<xsl:apply-templates select="type/@xmi:idref" mode="Description"/>
				</xsl:when>
				<xsl:when test="templateBinding">
					<xsl:apply-templates select="templateBinding/parameterSubstitution[1]/@actual" mode="Description"/>
				</xsl:when>
			</xsl:choose>
			<xsl:variable name="Cardinal">
				<xsl:call-template name="Cardinal"/>
			</xsl:variable>
			<xsl:attribute name="level">0</xsl:attribute>
			<xsl:attribute name="cardinal">
				<xsl:value-of select="$Cardinal"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="templateBinding">
					<xsl:apply-templates select="templateBinding" mode="Type"/>
					<!--xsl:copy-of select="templateBinding"/-->
				</xsl:when>
				<xsl:when test="starts-with(upperValue/@value,'[')">
					<xsl:apply-templates select="upperValue" mode="Array"/>
				</xsl:when>
				<xsl:when test="$Cardinal='0n' or $Cardinal='1n'">
					<xsl:element name="list">
						<xsl:attribute name="iterator">no</xsl:attribute>
						<xsl:choose>
							<xsl:when test="$Index!=''">
								<xsl:attribute name="index-idref">
									<xsl:value-of select="$Index"/>
								</xsl:attribute>
								<xsl:attribute name="type">indexed</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="type">simple</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:attribute name="desc">undef_template</xsl:attribute>
						<xsl:attribute name="level">0</xsl:attribute>
					</xsl:element>
				</xsl:when>
				<xsl:otherwise>
					<xsl:element name="get">
						<xsl:attribute name="by">val</xsl:attribute>
						<xsl:attribute name="modifier">var</xsl:attribute>
						<xsl:attribute name="range">no</xsl:attribute>
					</xsl:element>
					<xsl:element name="set">
						<xsl:attribute name="by">val</xsl:attribute>
						<xsl:attribute name="range">no</xsl:attribute>
					</xsl:element>
				</xsl:otherwise>
			</xsl:choose>
			<!--xsl:copy-of select="."/-->
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="upperValue" mode="Array">
		<xsl:element name="array">
			<xsl:attribute name="size">
				<xsl:value-of select="substring-before(substring(@value,2),']')"/>
			</xsl:attribute>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@type | @xmi:idref | @actual" mode="Description">
		<xsl:variable name="ID" select="."/>
		<xsl:choose>
			<xsl:when test="//*[(@xmi:type='uml:PrimitiveType' or @xmi:type='uml:DataType') and @xmi:id=$ID]">
				<xsl:attribute name="desc">
					<xsl:value-of select="//*[@xmi:id=$ID]/@name"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="idref">
					<xsl:value-of select="$ID"/>
				</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="@actual" mode="Index">
		<xsl:variable name="ID" select="."/>
		<xsl:choose>
			<xsl:when test="//*[(@xmi:type='uml:PrimitiveType' or @xmi:type='uml:DataType') and @xmi:id=$ID]">
				<xsl:attribute name="index-desc">
					<xsl:value-of select="//*[@xmi:id=$ID]/@name"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="index-idref">
					<xsl:value-of select="$ID"/>
				</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="templateBinding" mode="Type">
		<xsl:element name="list">
			<xsl:attribute name="iterator">no</xsl:attribute>
			<xsl:choose>
				<xsl:when test="parameterSubstitution[2]">
					<xsl:attribute name="type">indexed</xsl:attribute>
					<xsl:apply-templates select="parameterSubstitution[2]/@actual" mode="Index"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="type">simple</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:variable name="signatureID" select="@signature"/>
			<xsl:attribute name="idref">
				<xsl:value-of select="//ownedTemplateSignature[@xmi:id=$signatureID]/parent::*/@xmi:id"/>
			</xsl:attribute>
			<xsl:attribute name="level">0</xsl:attribute>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Cardinal">
		<xsl:variable name="LowerValue">
			<xsl:choose>
				<xsl:when test="not(lowerValue)">0</xsl:when>
				<xsl:when test="lowerValue[not(@value)]">0</xsl:when>
				<xsl:when test="lowerValue/@value=upperValue/@value">0</xsl:when>
				<xsl:when test="lowerValue[@value='*']">0</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="lowerValue/@value"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="Cardinal">
			<xsl:choose>
				<xsl:when test="starts-with(upperValue/@value,'[')">
					<xsl:choose>
						<xsl:when test="$LowerValue!=''">
							<xsl:value-of select="concat($LowerValue,'n')"/>
						</xsl:when>
						<xsl:otherwise>0n</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="not(upperValue) and $LowerValue='0'">01</xsl:when>
				<xsl:when test="upperValue[@value='*' and $LowerValue='']">0n</xsl:when>
				<xsl:when test="upperValue[@value='1' and $LowerValue='1']">1</xsl:when>
				<xsl:when test="upperValue[@value='*']">
					<xsl:value-of select="concat($LowerValue,'n')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="concat($LowerValue,upperValue/@value)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$Cardinal='00'">01</xsl:when>
			<xsl:when test="$Cardinal='0'">01</xsl:when>
			<xsl:when test="string-length($Cardinal)!=0">
				<xsl:value-of select="$Cardinal"/>
			</xsl:when>
			<xsl:otherwise>1</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Property">
		<xsl:element name="property">
			<xsl:attribute name="name">
				<xsl:value-of select="@name"/>
				<xsl:if test="not(@name) or @name=''">
					<xsl:value-of select="generate-id()"/>
				</xsl:if>
			</xsl:attribute>
			<xsl:attribute name="num-id">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="member">object</xsl:attribute>
			<xsl:element name="type" use-attribute-sets="Type">
				<xsl:choose>
					<xsl:when test="@type">
						<xsl:apply-templates select="@type" mode="Description"/>
					</xsl:when>
					<xsl:when test="type">
						<xsl:apply-templates select="type/@xmi:idref" mode="Description"/>
					</xsl:when>
				</xsl:choose>
			</xsl:element>
			<xsl:element name="variable">
				<xsl:attribute name="range">
					<xsl:value-of select="@visibility"/>
				</xsl:attribute>
			</xsl:element>
			<xsl:apply-templates select="." mode="Comment"/>
			<xsl:element name="get">
				<xsl:attribute name="by">val</xsl:attribute>
				<xsl:attribute name="modifier">var</xsl:attribute>
				<xsl:attribute name="range">no</xsl:attribute>
			</xsl:element>
			<xsl:element name="set">
				<xsl:attribute name="by">val</xsl:attribute>
				<xsl:attribute name="range">no</xsl:attribute>
			</xsl:element>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Parameter">
		<xsl:element name="param">
			<xsl:attribute name="name">
				<xsl:value-of select="@name"/>
				<xsl:if test="not(@name) or @name=''">
					<xsl:value-of select="generate-id()"/>
				</xsl:if>
			</xsl:attribute>
			<xsl:attribute name="num-id">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:element name="type" use-attribute-sets="Type">
				<xsl:choose>
					<xsl:when test="@type">
						<xsl:apply-templates select="@type" mode="Description"/>
					</xsl:when>
					<xsl:when test="type">
						<xsl:apply-templates select="type/@xmi:idref" mode="Description"/>
					</xsl:when>
				</xsl:choose>
			</xsl:element>
			<xsl:element name="variable">
				<xsl:attribute name="range">private</xsl:attribute>
			</xsl:element>
			<xsl:apply-templates select="." mode="Comment"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Return">
		<xsl:element name="return">
			<xsl:element name="type" use-attribute-sets="Type">
				<xsl:choose>
					<xsl:when test="@type">
						<xsl:apply-templates select="@type" mode="Description"/>
					</xsl:when>
					<xsl:when test="type">
						<xsl:apply-templates select="type/@xmi:idref" mode="Description"/>
					</xsl:when>
				</xsl:choose>
			</xsl:element>
			<!--xsl:apply-templates select="not(test>)"/-->
			<xsl:element name="variable">
				<xsl:attribute name="range">
					<xsl:value-of select="parent::*/@visibility"/>
				</xsl:attribute>
			</xsl:element>
			<xsl:apply-templates select="." mode="ReturnComment"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Method">
		<xsl:element name="method">
			<xsl:attribute name="name">
				<xsl:value-of select="@name"/>
				<xsl:if test="not(@name) or @name=''">
					<xsl:value-of select="generate-id()"/>
				</xsl:if>
			</xsl:attribute>
			<xsl:attribute name="num-id">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:attribute name="constructor">no</xsl:attribute>
			<xsl:attribute name="member">object</xsl:attribute>
			<xsl:call-template name="Implementation"/>
			<xsl:if test="not(ownedParameter[@direction='return'])">
				<xsl:attribute name="constructor">
					<xsl:value-of select="@visibility"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="ownedParameter[@direction='return']"/>
			<xsl:apply-templates select="." mode="Comment"/>
			<xsl:apply-templates select="ownedParameter[not(@direction) or @direction!='return']"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Class">
		<xsl:variable name="Name" select="@name"/>
		<xsl:element name="class" use-attribute-sets="Node">
			<xsl:attribute name="visibility">
				<xsl:choose>
					<xsl:when test="not(@visibility)">common</xsl:when>
					<xsl:when test="@visibility='package'">package</xsl:when>
					<xsl:otherwise>common</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="constructor">no</xsl:attribute>
			<xsl:attribute name="destructor">no</xsl:attribute>
			<xsl:apply-templates select="ownedOperation[@name=$Name]" mode="Constructor"/>
			<xsl:apply-templates select="ownedOperation[starts-with(@name,'~')]" mode="Destructor"/>
			<xsl:attribute name="inline">none</xsl:attribute>
			<xsl:call-template name="Implementation"/>
			<xsl:apply-templates select="ownedTemplateSignature"/>
			<xsl:apply-templates select="generalization | interfaceRealization"/>
			<xsl:apply-templates select="." mode="Comment"/>
			<xsl:apply-templates select="nestedClassifier"/>
			<xsl:apply-templates select="ownedAttribute"/>
			<xsl:apply-templates select="ownedOperation[@name!=$Name and not(starts-with(@name,'~'))]"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="ownedTemplateSignature">
		<xsl:for-each select="descendant::ownedParameteredElement">
			<xsl:variable name="Name" select="@name"/>
			<xsl:element name="model">
				<xsl:copy-of select="@name"/>
				<xsl:attribute name="id">
					<xsl:value-of select="ancestor::ownedTemplateSignature/parent::*/nestedClassifier[@name=$Name]/@xmi:id"/>
				</xsl:attribute>
			</xsl:element>
		</xsl:for-each>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="generalization">
		<xsl:element name="inherited">
			<xsl:attribute name="idref">
				<xsl:value-of select="@general"/>
			</xsl:attribute>
			<xsl:attribute name="range">public</xsl:attribute>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="interfaceRealization">
		<xsl:element name="inherited">
			<xsl:attribute name="idref">
				<xsl:value-of select="@supplier"/>
			</xsl:attribute>
			<xsl:attribute name="range">public</xsl:attribute>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Unknown">
		<xsl:element name="class">
			<xsl:attribute name="name">
				<xsl:value-of select="concat('undef_',translate(@name,' /([-|\@)]=}{#&amp;&quot;$£%ùµ*?./§,;:!&lt;&gt;','_'))"/>
			</xsl:attribute>
			<xsl:attribute name="id">
				<xsl:value-of select="@xmi:id"/>
			</xsl:attribute>
			<xsl:attribute name="implementation">simple</xsl:attribute>
			<xsl:attribute name="visibility">package</xsl:attribute>
			<xsl:attribute name="constructor">no</xsl:attribute>
			<xsl:attribute name="destructor">no</xsl:attribute>
			<xsl:attribute name="inline">none</xsl:attribute>
			<xsl:element name="comment">
				<xsl:apply-templates select="ownedComment"/>
			</xsl:element>
			<xsl:apply-templates select="*[not(self::ownedComment)]"/>
		</xsl:element>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template name="Implementation">
		<xsl:attribute name="implementation">
			<xsl:choose>
				<xsl:when test="@xmi:type='uml:Interface'">abstract</xsl:when>
				<xsl:when test="ownedTemplateSignature">container</xsl:when>
				<xsl:when test="@isLeaf='true'">final</xsl:when>
				<xsl:when test="@isAbstract='true'">abstract</xsl:when>
				<xsl:otherwise>simple</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>
	<!-- ======================================================================= -->
</xsl:stylesheet>




