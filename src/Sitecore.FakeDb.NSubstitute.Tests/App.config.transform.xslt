<?xml version="1.0" ?>
<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>

  <!-- FACTORIES -->
  <xsl:template match="/configuration/sitecore">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
      <xsl:copy-of select="document('App.config')/configuration/sitecore/factories" />
    </xsl:copy>
  </xsl:template>

</xsl:transform>