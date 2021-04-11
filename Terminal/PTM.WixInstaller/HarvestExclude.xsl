<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
    xmlns="http://schemas.microsoft.com/wix/2006/wi"

    version="1.0"
    exclude-result-prefixes="xsl wix"
>

  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" />

  <xsl:strip-space elements="*" />

  
  <xsl:key
      name="PdbToRemove"
      match="wix:Component[ substring( wix:File/@Source, string-length( wix:File/@Source ) - 3 ) = '.pdb' ]"
      use="@Id"
    />

  <xsl:key
    name="RunTimeDev"
    match="wix:Component[ substring( wix:File/@Source, string-length( wix:File/@Source ) - 8 ) = '.dev.json' ]"
    use="@Id"
    />

  <xsl:key
  name="Config"
  match="wix:Component[ substring( wix:File/@Source, string-length( wix:File/@Source ) - 6 ) = '.config' ]"
  use="@Id"
    />

  <xsl:key
  name="Terminal.Deps"
  match="wix:Component[ substring( wix:File/@Source, string-length( wix:File/@Source ) - 9 ) = '.deps.json' ]"
  use="@Id"
    />
  
  <!-- By default, copy all elements and nodes into the output... -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <!-- ...but if the element has the "ExeToRemove" key then don't render anything (i.e. removing it from the output) -->
  <xsl:template match="*[ self::wix:Component or self::wix:ComponentRef ][ key( 'PdbToRemove', @Id ) ]" />
  <xsl:template match="*[ self::wix:Component or self::wix:ComponentRef ][ key('RunTimeDev', @Id ) ]" />
  <xsl:template match="*[ self::wix:Component or self::wix:ComponentRef ][ key('Config', @Id ) ]" />
  <xsl:template match="*[ self::wix:Component or self::wix:ComponentRef ][ key('Terminal.Deps', @Id ) ]" />
</xsl:stylesheet>