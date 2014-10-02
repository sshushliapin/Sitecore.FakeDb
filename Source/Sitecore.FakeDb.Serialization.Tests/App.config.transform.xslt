<?xml version="1.0" ?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>

  <!-- SERIALIZATION FOLDERS -->
  <xsl:template match="/configuration/sitecore">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
      <xsl:copy-of select="document('App.config')/configuration/sitecore/szfolders" />
    </xsl:copy>
  </xsl:template>

  <!-- PIPELINES -->
  <xsl:template match="/configuration/sitecore/pipelines">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
      <xsl:copy-of select="document('App.config')/configuration/sitecore/pipelines/*" />
    </xsl:copy>
  </xsl:template>

</xsl:transform>