<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" indent="yes"/>
	<xsl:template match="programme">
		<xsl:element name="tste">
        	<xsl:for-each select="programm">
            	<xsl:value-of select="@name"/>
                <xsl:text> -> </xsl:text>
        		<xsl:value-of select="count(figuren/figur)"/>
                <br />
			</xsl:for-each>
        </xsl:element>
	</xsl:template>
</xsl:stylesheet>