<?xml version="1.0" ?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output omit-xml-declaration="yes" indent="yes"/>

  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="setting[@name='LicenseFile']">
    <setting name="LicenseFile" value="..\..\license.xml" />
  </xsl:template>

  <xsl:template match="runtime" />

</xsl:transform>