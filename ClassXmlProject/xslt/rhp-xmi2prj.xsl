<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0"
     xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
     xmlns:msxsl="urn:schemas-microsoft-com:xslt"
     xmlns:uml="http://schema.omg.org/spec/UML/2.1"
     xmlns:xmi="http://schema.omg.org/spec/XMI/2.1"
       >
<!-- ======================================================================= -->
  <xsl:output indent="yes" method="xml" encoding="utf-8" media-type="xprj"/>
  <xsl:param name="FolderDTD"/>
  <!-- ============================================================================== -->
  <xsl:attribute-set name="Type">
    <xsl:attribute name="level">0</xsl:attribute>
    <xsl:attribute name="by">val</xsl:attribute>
    <xsl:attribute name="modifier">var</xsl:attribute>
  </xsl:attribute-set>
  <!--  ============================================================================== -->
  <xsl:attribute-set name="Element">
    <xsl:attribute name="name">
      <xsl:value-of select="@name"/>
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
      <xsl:value-of select="@name"/>
    </xsl:attribute>
    <xsl:attribute name="id">
      <xsl:value-of select="@xmi:id"/>
    </xsl:attribute>
  </xsl:attribute-set>
  <!-- ============================================================================== -->
  <xsl:template match="/*">
    <xsl:if test="@xmi:version!='2.1'">
      <xsl:message terminate="yes">Expected XMI/UML 2.1 !</xsl:message>
    </xsl:if>
    <xsl:text>
</xsl:text>
    <xsl:text disable-output-escaping="yes">&lt;!DOCTYPE root SYSTEM "</xsl:text>
    <xsl:value-of select="$FolderDTD"/>
    <xsl:text disable-output-escaping="yes">class-model.dtd"&gt;</xsl:text>
    <xsl:text>
</xsl:text>
    <xsl:element name="root">
	  <!-- UML Designer DTD version -->
      <xsl:attribute name="version">1.3</xsl:attribute>
      <xsl:attribute name="name"><xsl:value-of select="*[name()='uml:Model']/@name"/></xsl:attribute>
      <xsl:text>
</xsl:text>
      <xsl:element name="generation">
        <xsl:attribute name="destination">C:\</xsl:attribute>
        <xsl:attribute name="language">0</xsl:attribute>
      </xsl:element>
      <xsl:text>
</xsl:text>
      <xsl:element name="comment">
        <xsl:variable name="ID" select="*[name()='uml:Model']/@xmi:id"/>
        <xsl:attribute name="brief"><xsl:value-of select="//*[@base_Package=$ID]/@description"/></xsl:attribute>
        <xsl:text>A detailed comment</xsl:text>
      </xsl:element>
      <xsl:apply-templates select="*[name()='uml:Model']/packagedElement[@xmi:type='uml:Class' and @xmi:type='uml:Interface']"/>
      <xsl:apply-templates select="*[name()='uml:Model']/packagedElement[@xmi:type='uml:Package']"/>
      <xsl:apply-templates select="//packagedElement[@xmi:type='uml:Association']"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="packagedElement">
    <xsl:choose>
      <xsl:when test="@xmi:type='uml:Package'">
        <xsl:call-template name="Package"/>
      </xsl:when>
      <xsl:when test="@xmi:type='uml:Class'">
        <xsl:call-template name="Class"/>
      </xsl:when>
      <xsl:when test="@xmi:type='uml:Interface'">
        <xsl:call-template name="Class"/>
      </xsl:when>
      <xsl:when test="@xmi:type='uml:Association'">
        <xsl:call-template name="Association"/>
      </xsl:when>
      <xsl:otherwise>
        <!--xsl:call-template name="Unknown"/-->
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="nestedClassifier">
    <xsl:text>
</xsl:text>
    <xsl:element name="typedef" use-attribute-sets="Node">
      <xsl:text>
</xsl:text>
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
      <xsl:text>
</xsl:text>
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
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedLiteral" mode="Enumeration">
    <xsl:text>
</xsl:text>
    <xsl:element name="enumvalue">
      <xsl:attribute name="id">
        <xsl:value-of select="@xmi:id"/>
      </xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:if test="specification[string-length(@value)!=0]">
        <xsl:attribute name="value">
          <xsl:value-of select="specification/@value"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="." mode="Comment"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedAttribute" mode="Element">
    <xsl:text>
</xsl:text>
    <xsl:element name="element" use-attribute-sets="Element">
      <xsl:apply-templates select="@type"/>
      <xsl:value-of select="ownedComment/@body"/>
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
      <xsl:when test="//packagedElement[@xmi:type='uml:PrimitiveType' and @xmi:id=$ID]">
        <xsl:attribute name="desc">
          <xsl:value-of select="//packagedElement[@xmi:type='uml:PrimitiveType' and @xmi:id=$ID]/@name"/>
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
    <xsl:text>
</xsl:text>
    <xsl:element name="package" use-attribute-sets="Node">
      <xsl:apply-templates select="." mode="Comment"/>
      <xsl:apply-templates select="packagedElement[@xmi:type!='uml:Association' and @xmi:type!='uml:Package']"/>
      <xsl:apply-templates select="packagedElement[@xmi:type='uml:Package']"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Association">
    <xsl:text>
</xsl:text>
    <xsl:element name="relationship">
      <xsl:variable name="ID" select="@xmi:id"/>
      <xsl:attribute name="id">
        <xsl:value-of select="$ID"/>
      </xsl:attribute>
      <xsl:attribute name="action">
        <xsl:apply-templates select="//ownedAttribute[@association=$ID][1]" mode="Action"/>
      </xsl:attribute>
      <xsl:attribute name="type">assembl</xsl:attribute>
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
        <xsl:when test="ownedEnd">
          <xsl:apply-templates select="ownedEnd" mode="Association">
            <xsl:with-param name="Association" select="$ID"/>
            <xsl:with-param name="Father">no</xsl:with-param>
            <xsl:with-param name="ChildName">father</xsl:with-param>
          </xsl:apply-templates>
          <xsl:apply-templates select="ownedEnd" mode="Association">
            <xsl:with-param name="Association" select="$ID"/>
            <xsl:with-param name="Father">yes</xsl:with-param>
            <xsl:with-param name="ChildName">child</xsl:with-param>
          </xsl:apply-templates>
        </xsl:when>
      </xsl:choose>
      <!--xsl:copy-of select="."/-->
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedEnd" mode="Association">
    <xsl:param name="Association"/>
    <xsl:param name="Father"/>
    <xsl:param name="ChildName"/>
    <xsl:choose>
      <xsl:when test="$Father='no'">
        <xsl:call-template name="NotNavigable">
          <xsl:with-param name="ChildName">father</xsl:with-param>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="Navigable">
          <xsl:with-param name="ChildName" select="$ChildName"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="NotNavigable">
    <xsl:param name="ChildName"/>
    <xsl:variable name="ID" select="parent::*/@xmi:id"/>
    <xsl:variable name="IDREF" select="//ownedAttribute[$ID=@association]/parent::*/@xmi:id"/>
    <xsl:text>
</xsl:text>
    <xsl:element name="{$ChildName}">
      <xsl:attribute name="name">NotNavigable</xsl:attribute>
      <xsl:attribute name="range">no</xsl:attribute>
      <xsl:attribute name="idref">
        <xsl:value-of select="@type"/>
      </xsl:attribute>
      <xsl:attribute name="cardinal">1</xsl:attribute>
      <xsl:attribute name="level">0</xsl:attribute>
      <xsl:text>
</xsl:text>
      <xsl:element name="get">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="modifier">var</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
      <xsl:text>
</xsl:text>
      <xsl:element name="set">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Navigable">
    <xsl:param name="ChildName"/>
    <xsl:variable name="ID" select="parent::*/@xmi:id"/>
    <xsl:apply-templates select="//ownedAttribute[$ID=@association]" mode="Association">
      <xsl:with-param name="ChildName" select="$ChildName"/>
    </xsl:apply-templates>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedAttribute" mode="Comment">
    <xsl:variable name="ID" select="@xmi:id"/>
    <xsl:element name="comment">
      <xsl:value-of select="//*[@base_Property=$ID]/@description"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="nestedClassifier" mode="CommentDataType">
    <xsl:variable name="ID" select="@xmi:id"/>
    <xsl:element name="comment">
      <xsl:value-of select="//*[@base_DataType=$ID]/@description"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedLiteral" mode="Comment">
    <xsl:variable name="ID" select="@xmi:id"/>
    <xsl:value-of select="//*[@base_EnumerationLiteral=$ID]/@description"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedParameter" mode="Comment">
    <xsl:variable name="ID" select="@xmi:id"/>
    <xsl:element name="comment">
      <xsl:value-of select="//*[@base_Parameter=$ID]/@description"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedParameter" mode="ReturnComment">
    <xsl:variable name="ID" select="@xmi:id"/>
    <xsl:element name="comment">
      <xsl:value-of select="//*[@base_Parameter=$ID]/@description"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedAttribute" mode="Action">
    <xsl:variable name="ID" select="@xmi:id"/>
    <xsl:if test="position()=1">
      <xsl:value-of select="//*[@base_Property=$ID]/@description"/>
    </xsl:if>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedOperation" mode="Comment">
    <xsl:element name="comment">
      <xsl:variable name="ID" select="@xmi:id"/>
      <xsl:attribute name="brief">
       <xsl:value-of select="//*[@base_Operation=$ID]/@description"/>
      </xsl:attribute>
      <xsl:text>A detailed comment</xsl:text>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="packagedElement" mode="Comment">
    <xsl:element name="comment">
      <xsl:variable name="ID" select="@xmi:id"/>
      <xsl:attribute name="brief">
       <xsl:choose>
         <xsl:when test="@xmi:type='uml:Class'">
           <xsl:value-of select="//*[@base_Class=$ID]/@description"/>
         </xsl:when>
         <xsl:when test="@xmi:type='uml:Package'">
           <xsl:value-of select="//*[@base_Package=$ID]/@description"/>
         </xsl:when>
       </xsl:choose>
      </xsl:attribute>
      <xsl:text>A detailed comment</xsl:text>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="ownedAttribute" mode="Association">
    <xsl:param name="ChildName"/>
    <xsl:text>
</xsl:text>
    <xsl:element name="{$ChildName}">
      <xsl:attribute name="name">
        <xsl:if test="string-length(@name)=0">UnknownMember</xsl:if>
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="range">
        <xsl:value-of select="@visibility"/>
      </xsl:attribute>
      <xsl:attribute name="idref">
        <xsl:value-of select="@type"/>
      </xsl:attribute>
      <xsl:attribute name="level">0</xsl:attribute>
      <xsl:attribute name="cardinal">
        <xsl:call-template name="Cardinal"/>
      </xsl:attribute>
      <xsl:text>
</xsl:text>
      <xsl:element name="get">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="modifier">var</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
      <xsl:text>
</xsl:text>
      <xsl:element name="set">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
      <!--xsl:copy-of select="."/-->
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Cardinal">
    <xsl:variable name="LowerValue">
      <xsl:choose>
        <xsl:when test="defaultValue">
          <xsl:value-of select="defaultValue/@value"/>
        </xsl:when>
        <xsl:when test="not(lowerValue)"/>
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
      <xsl:when test="string-length($Cardinal)!=0">
        <xsl:value-of select="$Cardinal"/>
      </xsl:when>
      <xsl:otherwise>1</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Property">
    <xsl:text>
</xsl:text>
    <xsl:element name="property">
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="num-id">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="member">object</xsl:attribute>
      <xsl:element name="type" use-attribute-sets="Type">
        <xsl:apply-templates select="@type"/>
      </xsl:element>
      <xsl:text>
</xsl:text>
      <xsl:element name="variable">
        <xsl:attribute name="range">
          <xsl:value-of select="@visibility"/>
        </xsl:attribute>
      </xsl:element>
      <xsl:apply-templates select="." mode="Comment"/>
      <xsl:text>
</xsl:text>
      <xsl:element name="get">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="modifier">var</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
      <xsl:text>
</xsl:text>
      <xsl:element name="set">
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="range">no</xsl:attribute>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Parameter">
    <xsl:text>
</xsl:text>
    <xsl:element name="param">
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
      </xsl:attribute>
      <xsl:attribute name="num-id">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:element name="type" use-attribute-sets="Type">
        <xsl:apply-templates select="@type"/>
      </xsl:element>
      <xsl:text>
</xsl:text>
      <xsl:element name="variable">
        <xsl:attribute name="range">private</xsl:attribute>
      </xsl:element>
        <xsl:apply-templates select="." mode="Comment"/>
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Return">
    <xsl:text>
</xsl:text>
    <xsl:element name="return">
      <xsl:element name="type" use-attribute-sets="Type">
        <xsl:apply-templates select="@type"/>
      </xsl:element>
      <xsl:apply-templates/>
      <xsl:text>
</xsl:text>
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
    <xsl:text>
</xsl:text>
    <xsl:element name="method">
      <xsl:attribute name="name">
        <xsl:value-of select="@name"/>
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
    <xsl:text>
</xsl:text>
    <xsl:element name="class" use-attribute-sets="Node">
      <xsl:copy-of select="@visibility"/>
      <xsl:if test="not(@visibility)">
        <xsl:attribute name="visibility">package</xsl:attribute>
      </xsl:if>
      <xsl:attribute name="constructor">no</xsl:attribute>
      <xsl:attribute name="destructor">no</xsl:attribute>
      <xsl:apply-templates select="ownedOperation[@name=$Name]" mode="Constructor"/>
      <xsl:apply-templates select="ownedOperation[starts-with(@name,'~')]" mode="Destructor"/>
      <xsl:attribute name="inline">none</xsl:attribute>
      <xsl:call-template name="Implementation"/>
      <xsl:apply-templates select="generalization | interfaceRealization"/>
      <xsl:apply-templates select="ownedComment"/>
      <xsl:apply-templates select="." mode="Comment"/>
      <xsl:apply-templates select="nestedClassifier"/>
      <xsl:apply-templates select="ownedAttribute"/>
      <xsl:apply-templates select="ownedOperation[@name!=$Name and not(starts-with(@name,'~'))]"/>
    </xsl:element>
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
    </xsl:element>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Implementation">
    <xsl:attribute name="implementation">
      <xsl:choose>
        <xsl:when test="@isAbstract='true'">abstract</xsl:when>
        <xsl:otherwise>simple</xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
</xsl:stylesheet>








