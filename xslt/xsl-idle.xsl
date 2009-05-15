<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
       xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output indent="yes" method="xml"/>
  <xsl:template match="/* | @* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>
  
  <xsl:template match="@*"><xsl:copy/></xsl:template>
  
  <xsl:template match="text()" xml:space="preserve">
    <xsl:value-of select="." xml:space="preserve"/>
  </xsl:template>
</xsl:stylesheet>

