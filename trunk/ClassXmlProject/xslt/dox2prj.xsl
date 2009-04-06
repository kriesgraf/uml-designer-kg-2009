<?xml version="1.0" encoding="utf-8"?>
<!--  UML Designer DTD Version 1.2, Doxygen tool version 1.5.7.1 and above -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- ======================================================================= -->
  <xsl:output indent="yes" method="xml" encoding="utf-8"/>
  <!-- ======================================================================= -->
  <xsl:include href="combine.xsl"/>
  <!-- ======================================================================= -->
  <xsl:key match="includes" name="include" use="text()"/>
  <!--============================================================================== -->
  <xsl:param name="DoxFolder"/>
  <!-- ============================================================================= -->
  <xsl:template match="/*">
    <xsl:if test="name()!='doxygenindex'">
      <xsl:message terminate="yes">This file is not a Doxygen index file !</xsl:message>
    </xsl:if>
    <xsl:variable name="DoxygenIndex">
      <xsl:for-each select="compound[@kind!='dir' and @kind!='file']">
        <xsl:copy-of select="document( concat($DoxFolder,@refid,'.xml'))/doxygen/*"/>
      </xsl:for-each>
    </xsl:variable>
    <xsl:variable name="Doxygen">
      <doxygen version="{@version}">
        <xsl:for-each select="msxsl:node-set($DoxygenIndex)//compounddef[@kind='struct' or @kind='union']">
          <xsl:if test="not(@id=//compounddef[@kind='class']/innerclass/@refid)">
            <xsl:apply-templates select="." mode="DoxygenIndex"/>
          </xsl:if>
        </xsl:for-each>
        <xsl:apply-templates select="msxsl:node-set($DoxygenIndex)//compounddef[@kind='class']" mode="Doxygen"/>
        <xsl:apply-templates select="msxsl:node-set($DoxygenIndex)//compounddef[@kind='namespace']" mode="Doxygen"/>
      </doxygen>
    </xsl:variable>
    <!--xsl:copy-of select="$Doxygen"/-->
    <xsl:apply-templates select="msxsl:node-set($Doxygen)" mode="Main"/>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="doxygen" mode="Main">
    <xsl:element name="root">
	  <!-- UMl Disigner DTD vesion -->
      <xsl:attribute name="version">1.3</xsl:attribute>
      <xsl:attribute name="name">Doxygen_project</xsl:attribute>
      <xsl:element name="generation">
        <xsl:apply-templates select="/doxygen/compounddef[@kind='class'][1]" mode="Location"/>
      </xsl:element>
      <xsl:element name="comment">
        <xsl:attribute name="brief">A brief comment</xsl:attribute>A detailed comment
      </xsl:element>
      <xsl:call-template name="StructRef"/>
      <xsl:apply-templates select="/doxygen/compounddef[@kind='class']" mode="Index"/>
      <xsl:apply-templates select="/doxygen/compounddef[@kind='namespace']" mode="Index"/>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="compounddef" mode="Location">
    <location kind="{@kind}" name="{compoundname}">
      <xsl:value-of select="location/@file"/>
    </location>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="compounddef[@kind='class']" mode="Index">
    <xsl:apply-templates select="." mode="Class"/>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="compounddef[@kind='namespace']" mode="Index">
    <xsl:variable name="PackageName" select="compoundname"/>
    <xsl:element name="package">
      <xsl:attribute name="name">
        <xsl:value-of select="$PackageName"/>
      </xsl:attribute>
      <xsl:attribute name="id">
        <xsl:value-of select="@id"/>
      </xsl:attribute>
      <xsl:apply-templates select="compounddef[@kind='class'][1]" mode="Location"/>
      <xsl:element name="comment">
        <xsl:copy-of select="briefdescription | detaileddescription"/>
        <!--xsl:attribute name="brief">
          <xsl:value-of select="briefdescription"/>
        </xsl:attribute>
        <xsl:value-of select="detaileddescription"/-->
      </xsl:element>
      <xsl:apply-templates select="compounddef" mode="Class"/>
      <xsl:apply-templates select="sectiondef" mode="Class">
        <xsl:with-param name="PackageName" select="$PackageName"/>
      </xsl:apply-templates>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="sectiondef" mode="Class">
    <xsl:param name="PackageName"/>
    <xsl:element name="class">
      <xsl:attribute name="implementation">abstract</xsl:attribute>
      <xsl:attribute name="visibility">package</xsl:attribute>
      <xsl:attribute name="constructor">no</xsl:attribute>
      <xsl:attribute name="destructor">no</xsl:attribute>
      <xsl:attribute name="inline">none</xsl:attribute>
      <xsl:attribute name="id">class
        <xsl:value-of select="generate-id()"/>
      </xsl:attribute>
      <xsl:attribute name="name">enum_
        <xsl:value-of select="$PackageName"/>
      </xsl:attribute>
      <xsl:attribute name="destructor">no</xsl:attribute>
      <xsl:element name="comment">
        <xsl:attribute name="brief">A brand new class</xsl:attribute>A multi-lines comment here
      </xsl:element>
      <xsl:apply-templates select="memberdef[@kind!='variable' and @kind!='function']" mode="OwnerElements"/>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="compounddef" mode="Class">
    <xsl:variable name="ClassName">
      <xsl:if test="contains(compoundname,'::')">
        <xsl:value-of select="substring-after(compoundname,'::')"/>
      </xsl:if>
      <xsl:if test="not(contains(compoundname,'::'))">
        <xsl:value-of select="compoundname"/>
      </xsl:if>
    </xsl:variable>
    <xsl:element name="class">
      <xsl:attribute name="constructor">no</xsl:attribute>
      <xsl:attribute name="destructor">public</xsl:attribute>
      <xsl:attribute name="visibility">package</xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="$ClassName"/>
      </xsl:attribute>
      <xsl:attribute name="id">
        <xsl:value-of select="@id"/>
      </xsl:attribute>
      <xsl:variable name="Inline">
        <xsl:apply-templates select="descendant::memberdef[contains(name,$ClassName)]" mode="Inline"/>
      </xsl:variable>
      <xsl:attribute name="inline">
				<!--xsl:value-of select="$Inline"/-->
        <xsl:choose>
          <xsl:when test="$Inline='constructordestructor'">both</xsl:when>
          <xsl:when test="$Inline='destructorconstructor'">both</xsl:when>
          <xsl:when test="string-length($Inline)=0">none</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$Inline"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:variable name="Implementation">
        <xsl:apply-templates select="descendant::memberdef[not(contains(name,$ClassName))]" mode="Implementation"/>
      </xsl:variable>
      <xsl:attribute name="implementation">
        <xsl:choose>
          <xsl:when test="templateparamlist/param/type">container</xsl:when>
          <xsl:when test="contains($Implementation,'_pure-virtual;') and not(contains($Implementation,'_non-virtual;')) and not(contains($Implementation,'_virtual;'))">abstract</xsl:when>
          <xsl:when test="contains($Implementation,'_virtual;')">virtual</xsl:when>
          <xsl:otherwise>simple</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <!--BEGIN  Some request insert attribute in class element, if some sub-element are inserted before, next attributes are lost ! -->
      <xsl:apply-templates select="descendant::memberdef[contains(name,$ClassName)]" mode="Construction"/>
      <!--END  Some request insert attribute in class element, if some sub-element are inserted before, next attributes are lost ! -->
      <xsl:for-each select="templateparamlist/param/type">
        <xsl:element name="model">
          <xsl:attribute name="name">
            <xsl:value-of select="substring-after(.,'class ')"/>
          </xsl:attribute>
        </xsl:element>
      </xsl:for-each>
      <xsl:apply-templates select="basecompoundref" mode="Inherited"/>
      <xsl:apply-templates select="includes" mode="Dependencies">
        <xsl:with-param name="Inheritance">
          <xsl:apply-templates select="innerclass" mode="Inheritance"/>
        </xsl:with-param>
      </xsl:apply-templates>
      <xsl:element name="comment">
        <xsl:copy-of select="briefdescription | detaileddescription"/>
        <!--xsl:attribute name="brief">
          <xsl:value-of select="briefdescription/descendant::text()" xml:space="default"/>
        </xsl:attribute>
        <xsl:value-of select="detaileddescription/descendant::text()"/-->
              </xsl:element>
      <!--xsl:call-template name="Imports"/-->
      <xsl:apply-templates select="descendant::compounddef" mode="ClassMode">
        <xsl:with-param name="ClassName" select="$ClassName"/>
      </xsl:apply-templates>
      <xsl:apply-templates select="descendant::memberdef[@kind!='variable' and @kind!='function']" mode="OwnerElements"/>
      <xsl:apply-templates select="descendant::memberdef[@kind='variable']" mode="OwnerElements"/>
      <xsl:apply-templates select="descendant::memberdef[@kind='function' and not(contains(name,'~'))]" mode="OwnerElements">
        <xsl:with-param name="ClassName" select="$ClassName"/>
      </xsl:apply-templates>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="memberdef" mode="Inline">
    <xsl:if test="@kind='function'">
      <xsl:choose>
        <xsl:when test="contains(name,'~')">
          <xsl:if test="@inline='yes'">destructor</xsl:if>
        </xsl:when>
        <xsl:when test="param">
					<!-- Ignored -->
				</xsl:when>
        <xsl:otherwise>
          <xsl:if test="@inline='yes'">constructor</xsl:if>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="memberdef" mode="Implementation">
    <xsl:if test="@kind='function'">
      <xsl:value-of select="concat('_',@virt,';')"/>
    </xsl:if>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="memberdef" mode="Construction">
    <xsl:if test="@kind='function'">
      <xsl:choose>
        <xsl:when test="contains(name,'~')">
          <xsl:attribute name="destructor">
            <xsl:value-of select="@prot"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="param and not(declname)">
          <xsl:attribute name="constructor">
            <xsl:value-of select="@prot"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="constructor">
            <xsl:value-of select="@prot"/>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="basecompoundref" mode="Inherited">
    <xsl:element name="inherited">
      <xsl:attribute name="idref">
        <xsl:value-of select="@refid"/>
      </xsl:attribute>
      <xsl:attribute name="range">
        <xsl:value-of select="@prot"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="includes" mode="Dependencies">
    <xsl:param name="Inheritance"/>
    <xsl:apply-templates select="//node[label=current()/text()]">
      <xsl:with-param name="Inheritance" select="$Inheritance"/>
    </xsl:apply-templates>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="memberdef" mode="OwnerElements">
    <xsl:choose>
      <xsl:when test="@kind='function' and string-length(type)=0">
        <xsl:if test="param/declname">
          <xsl:call-template name="Method"/>
        </xsl:if>
      </xsl:when>
      <xsl:when test="@kind='function'">
        <xsl:call-template name="Method"/>
      </xsl:when>
      <xsl:when test="@kind='variable'">
        <xsl:element name="property">
          <xsl:attribute name="name">
            <xsl:value-of select="name"/>
          </xsl:attribute>
          <xsl:attribute name="num-id">
            <xsl:value-of select="position()"/>
          </xsl:attribute>
          <xsl:attribute name="member">
            <xsl:if test="@static='yes'">class</xsl:if>
            <xsl:if test="@static='no'">object</xsl:if>
          </xsl:attribute>
          <xsl:copy-of select="type"/>
          <xsl:call-template name="Range"/>
          <xsl:element name="comment">
            <xsl:copy-of select="briefdescription | detaileddescription"/>
            <!--xsl:value-of select="briefdescription/descendant::text()"/-->
          </xsl:element>
          <xsl:element name="get">
            <xsl:attribute name="range">no</xsl:attribute>
            <xsl:attribute name="by">val</xsl:attribute>
            <xsl:attribute name="modifier">var</xsl:attribute>
          </xsl:element>
          <xsl:element name="set">
            <xsl:attribute name="range">no</xsl:attribute>
            <xsl:attribute name="by">val</xsl:attribute>
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="typedef">
          <xsl:attribute name="name">
            <xsl:value-of select="name"/>
          </xsl:attribute>
          <xsl:attribute name="id">
            <xsl:value-of select="@id"/>
          </xsl:attribute>
          <xsl:attribute name="id">
            <xsl:value-of select="@id"/>
          </xsl:attribute>
          <xsl:copy-of select="type"/>
          <xsl:call-template name="Range"/>
          <xsl:element name="comment">
            <xsl:copy-of select="briefdescription | detaileddescription"/>
            <!--xsl:value-of select="briefdescription/descendant::text()"/-->
          </xsl:element>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="compounddef" mode="ClassMode">
    <xsl:param name="ClassName"/>
    <xsl:element name="typedef">
      <xsl:attribute name="name">
        <xsl:value-of select="substring-after(compoundname,concat($ClassName,'::'))"/>
      </xsl:attribute>
      <xsl:attribute name="id">
        <xsl:value-of select="@id"/>
      </xsl:attribute>
      <xsl:element name="type">
        <xsl:attribute name="modifier">var</xsl:attribute>
        <xsl:attribute name="level">0</xsl:attribute>
        <xsl:attribute name="by">val</xsl:attribute>
        <xsl:attribute name="struct">
          <xsl:value-of select="@kind"/>
        </xsl:attribute>
        <xsl:apply-templates select="descendant::memberdef[@kind='variable']" mode="Structure"/>
      </xsl:element>
      <xsl:call-template name="Range"/>
      <xsl:element name="comment">
        <xsl:copy-of select="briefdescription | detaileddescription"/>
        <!--xsl:value-of select="briefdescription"/-->
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="memberdef" mode="Structure">
    <xsl:element name="element">
      <xsl:attribute name="num-id">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:attribute name="name">
        <xsl:value-of select="name"/>
      </xsl:attribute>
      <xsl:copy-of select="type"/>
      <!--xsl:copy-of select="argsstring"/-->
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="type">
    <xsl:value-of select="text()"/>
    <xsl:if test="ref">
      <xsl:value-of select="ref/text()"/>
    </xsl:if>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template name="Range">
    <xsl:param name="DefaultValueOk"/>
    <xsl:element name="variable">
      <xsl:attribute name="range">private</xsl:attribute>
      <xsl:if test="@prot">
        <xsl:attribute name="range">
          <xsl:value-of select="@prot"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="initializer">
        <xsl:if test="ref">
          <xsl:attribute name="valref">
            <xsl:value-of select="initializer/ref/@refid"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:if test="not(ref)">
          <xsl:attribute name="value">
            <xsl:value-of select="initializer"/>
          </xsl:attribute>
        </xsl:if>
      </xsl:if>
      <xsl:if test="$DefaultValueOk!='Ok'">
        <!--xsl:copy-of select="argsstring"/-->
      </xsl:if>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="memberdef" mode="OwnerElements">
		<!--xsl:copy-of select="."/-->
    <xsl:choose>
      <xsl:when test="@kind='function' and string-length(type)=0">
        <xsl:if test="param/declname">
          <xsl:call-template name="Method"/>
        </xsl:if>
      </xsl:when>
      <xsl:when test="@kind='function'">
        <xsl:call-template name="Method"/>
      </xsl:when>
      <xsl:when test="@kind='variable'">
        <xsl:comment>memberdef-variable:
          <xsl:value-of select="@id"/>
        </xsl:comment>
        <xsl:element name="property">
          <xsl:attribute name="name">
            <xsl:value-of select="name"/>
          </xsl:attribute>
          <xsl:attribute name="num-id">
            <xsl:value-of select="position()"/>
          </xsl:attribute>
          <xsl:attribute name="member">
            <xsl:if test="@static='yes'">class</xsl:if>
            <xsl:if test="@static='no'">object</xsl:if>
          </xsl:attribute>
          <xsl:copy-of select="type"/>
          <xsl:call-template name="Range"/>
          <xsl:element name="comment">
            <xsl:copy-of select="briefdescription | detaileddescription"/>
            <!--xsl:value-of select="briefdescription/descendant::text()"/-->
          </xsl:element>
          <xsl:element name="get">
            <xsl:attribute name="range">no</xsl:attribute>
            <xsl:attribute name="by">val</xsl:attribute>
            <xsl:attribute name="modifier">var</xsl:attribute>
          </xsl:element>
          <xsl:element name="set">
            <xsl:attribute name="range">no</xsl:attribute>
            <xsl:attribute name="by">val</xsl:attribute>
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:when test="@kind='enum'">
        <xsl:element name="typedef">
          <xsl:attribute name="name">
            <xsl:value-of select="name"/>
          </xsl:attribute>
          <xsl:attribute name="id">
            <xsl:value-of select="@id"/>
          </xsl:attribute>
          <xsl:element name="type">
            <xsl:attribute name="modifier">var</xsl:attribute>
            <xsl:attribute name="level">0</xsl:attribute>
            <xsl:attribute name="by">val</xsl:attribute>
            <xsl:apply-templates select="enumvalue"/>
          </xsl:element>
          <xsl:call-template name="Range"/>
          <xsl:element name="comment">
            <xsl:copy-of select="briefdescription | detaileddescription"/>
            <!--xsl:value-of select="briefdescription/descendant::text()"/-->
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="typedef">
          <xsl:attribute name="name">
            <xsl:value-of select="name"/>
          </xsl:attribute>
          <xsl:attribute name="id">
            <xsl:value-of select="@id"/>
          </xsl:attribute>
          <xsl:attribute name="id">
            <xsl:value-of select="@id"/>
          </xsl:attribute>
          <xsl:copy-of select="type"/>
          <xsl:call-template name="Range"/>
          <xsl:element name="comment">
            <xsl:copy-of select="briefdescription | detaileddescription"/>
            <!--xsl:value-of select="briefdescription/descendant::text()"/-->
          </xsl:element>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="enumvalue">
      <!--xsl:copy-of select="."/-->
    <xsl:element name="enumvalue">
      <xsl:attribute name="name">
        <xsl:value-of select="name"/>
      </xsl:attribute>
      <xsl:attribute name="value">
        <xsl:value-of select="initializer"/>
      </xsl:attribute>
      <xsl:value-of select="briefdescription"/>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template name="Method">
    <xsl:element name="method">
      <xsl:if test="string-length(type)!=0">
        <xsl:attribute name="constructor">no</xsl:attribute>
        <xsl:attribute name="num-id">
          <xsl:value-of select="position()"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="string-length(type)=0 and param/declname">
        <xsl:attribute name="constructor">
          <xsl:value-of select="@prot"/>
        </xsl:attribute>
        <xsl:attribute name="num-id">
          <xsl:value-of select="concat('CONST',position())"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="name" mode="MethodName"/>
      <xsl:attribute name="member">
        <xsl:if test="@static='yes'">class</xsl:if>
        <xsl:if test="@static='no'">object</xsl:if>
      </xsl:attribute>
      <xsl:attribute name="implementation">
        <xsl:choose>
          <xsl:when test="@virt='pure-virtual'">abstract</xsl:when>
          <xsl:when test="@virt='virtual'">virtual</xsl:when>
          <xsl:otherwise>simple</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:if test="string-length(type)!=0">
        <xsl:element name="return">
          <xsl:element name="type">
            <xsl:copy-of select="type/node()"/>
          </xsl:element>
          <!--xsl:copy-of select="argsstring"/-->
          <xsl:call-template name="Range">
            <xsl:with-param name="DefaultValueOk">Ok</xsl:with-param>
          </xsl:call-template>
          <xsl:element name="comment">
            <xsl:value-of select="descendant::simplesect[@kind='return']/descendant::text()"/>
          </xsl:element>
        </xsl:element>
      </xsl:if>
      <xsl:element name="comment">
        <xsl:copy-of select="briefdescription"/>
        <!--xsl:attribute name="brief">
          <xsl:value-of select="briefdescription/descendant::text()"/>
        </xsl:attribute-->
        <xsl:apply-templates select="detaileddescription" mode="MethodComment"/>
      </xsl:element>
      <xsl:apply-templates select="param[declname]"/>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="node()" mode="MethodComment">
    <xsl:copy>
      <xsl:apply-templates select="node()[not(self::parameterlist) and not(self::simplesect)]" mode="MethodComment"/>
    </xsl:copy>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="name" mode="MethodName">
    <xsl:variable name="Name">
      <xsl:if test="contains(.,'::')">
        <xsl:value-of select="substring-after(.,'::')"/>
      </xsl:if>
      <xsl:if test="not(contains(.,'::'))">
        <xsl:value-of select="."/>
      </xsl:if>
    </xsl:variable>
    <xsl:if test="substring($Name,1,8)='operator'">
      <xsl:attribute name="name">operator</xsl:attribute>
      <xsl:attribute name="operator">
        <xsl:value-of select="substring-after($Name,'operator')"/>
      </xsl:attribute>
    </xsl:if>
    <xsl:if test="substring($Name,1,8)!='operator'">
      <xsl:attribute name="name">
        <xsl:value-of select="$Name"/>
      </xsl:attribute>
    </xsl:if>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template match="param">
    <xsl:element name="param">
      <xsl:attribute name="name">
        <xsl:value-of select="declname"/>
      </xsl:attribute>
      <xsl:attribute name="num-id">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:copy-of select="type"/>
      <!--xsl:copy-of select="argsstring"/-->
      <xsl:call-template name="Range"/>
      <xsl:element name="comment">
        <xsl:copy-of select="parent::*/descendant::parameterlist[@kind='param']/parameteritem[parameternamelist/parametername=current()/declname]/parameterdescription"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <!--  ================================================================== -->
  <xsl:template name="StructRef">
    <xsl:for-each select="/doxygen/compounddef[@kind='struct' or @kind='union']/includes[generate-id()=generate-id(key('include',text())[1])]">
      <xsl:element name="import">
        <xsl:attribute name="name">
          <xsl:value-of select="concat('Source',position())"/>
        </xsl:attribute>
        <xsl:attribute name="param">
          <xsl:value-of select="text()"/>
        </xsl:attribute>
        <xsl:attribute name="visibility">package</xsl:attribute>
        <xsl:element name="export">
          <xsl:attribute name="name">
            <xsl:value-of select="concat('Source',position())"/>
          </xsl:attribute>
          <xsl:attribute name="source">
            <xsl:value-of select="text()"/>
          </xsl:attribute>
          <xsl:for-each select="//compounddef[@kind='struct' or @kind='union'][includes/text()=current()/text()]">
            <xsl:element name="reference">
              <xsl:attribute name="name">
                <xsl:value-of select="compoundname"/>
              </xsl:attribute>
              <xsl:attribute name="id">
                <xsl:value-of select="@id"/>
              </xsl:attribute>
              <xsl:attribute name="type">typedef</xsl:attribute>
            </xsl:element>
          </xsl:for-each>
        </xsl:element>
      </xsl:element>
    </xsl:for-each>
  </xsl:template>
  <!--  ================================================================== -->
</xsl:stylesheet>


























