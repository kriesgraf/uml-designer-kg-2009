<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!--============================================================================== -->
	<xsl:output method="xml" version="1.0" indent="yes" standalone="yes"/>
	<!--============================================================================== -->
	<xsl:param name="FolderTest"/>
	<!--============================================================================== -->
	<xsl:template match="/*">
    	<xsl:variable name="Doxygen">
    		<xsl:for-each select="compound[@kind!='dir' and @kind!='file']">
    			<xsl:copy-of select="document( concat($FolderTest,@refid,'.xml'))/doxygen/*"/></xsl:for-each>
    	</xsl:variable>
    	<rt>
        <xsl:apply-templates select="msxsl:node-set($Doxygen)//compounddef[@kind='class']" mode="Doxygen"/>
        <xsl:apply-templates select="msxsl:node-set($Doxygen)//compounddef[@kind='namespace']" mode="Doxygen"/>
        </rt>
	</xsl:template>
	<!--============================================================================== -->
	<xsl:template match="compounddef[@kind='struct' or @kind='union']" mode="Doxygen">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:copy-of select="compoundname"/>
			<xsl:copy-of select="includes"/>
		</xsl:copy>
	</xsl:template>
	<!--============================================================================== -->
	<xsl:template match="compounddef[@kind='class']" mode="Doxygen">
		<xsl:if test="not(@id=//compounddef[@kind='namespace']/innerclass/@refid)">
			<xsl:if test="not(@id=//compounddef[@kind='class']/innerclass/@refid)">
				<xsl:apply-templates select="." mode="DoxygenClass"/>
			</xsl:if>
		</xsl:if>
	</xsl:template>
	<!--============================================================================== -->
	<xsl:template match="compounddef[@kind='namespace']" mode="Doxygen">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:copy-of select="compoundname"/>
			<xsl:for-each select="innerclass">
				<xsl:variable name="ID" select="@refid"/>
				<xsl:apply-templates select="//compounddef[@id=$ID]" mode="DoxygenClass"/>
			</xsl:for-each>
			<xsl:copy-of select="sectiondef"/>
			<xsl:copy-of select="briefdescription"/>
			<xsl:copy-of select="detaileddescription"/>
		</xsl:copy>
	</xsl:template>
	<!--============================================================================== -->
	<xsl:template match="compounddef" mode="DoxygenClass">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:copy-of select="compoundname"/>
			<xsl:copy-of select="templateparamlist"/>
			<xsl:copy-of select="basecompoundref"/>
			<xsl:copy-of select="includes"/>
			<xsl:for-each select="innerclass">
				<xsl:variable name="ID" select="@refid"/>
				<xsl:apply-templates select="//compounddef[@id=$ID]" mode="DoxygenClass"/>
			</xsl:for-each>
			<xsl:copy-of select="sectiondef"/>
			<xsl:copy-of select="briefdescription"/>
			<xsl:copy-of select="detaileddescription"/>
			<xsl:copy-of select="location"/>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>





