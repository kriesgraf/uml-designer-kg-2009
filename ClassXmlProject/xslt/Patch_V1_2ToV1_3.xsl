<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- ======================================================================= -->
  <xsl:output indent="yes" method="xml" encoding="utf-8"/>
  <xsl:param name="FolderDTD"/>
  <!-- ======================================================================= -->
  <xsl:template match="/root">
  <!-- &#13;&#10; -->
    <xsl:text>
</xsl:text>
    <xsl:text disable-output-escaping="yes">&lt;!DOCTYPE root SYSTEM "</xsl:text>
    <xsl:value-of select="$FolderDTD"/>
    <xsl:text disable-output-escaping="yes">class-model.dtd"&gt;</xsl:text>
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:attribute name="version">1.3</xsl:attribute>
      <xsl:call-template name="Name"/>
      <xsl:apply-templates select="generation[1]"/>
      <xsl:apply-templates select="comment[1]"/>
      <xsl:apply-templates select="import"/>
      <xsl:apply-templates select="class"/>
      <xsl:apply-templates select="package"/>
      <xsl:apply-templates select="relationship"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="generation">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:choose>
        <xsl:when test="not(@language) or string-length(@language)=0">
          <xsl:attribute name="language">0</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@language"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:choose>
        <xsl:when test="not(@destination) or string-length(@destination)=0">
          <xsl:attribute name="destination">C:\</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@destination"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="comment">
    <xsl:text>
</xsl:text>
    <xsl:choose>
      <xsl:when test="parent::property | parent::param | parent::return">
        <xsl:copy-of select="."/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy>
          <xsl:if test="not(@brief)">
            <xsl:attribute name="brief">Brief comment</xsl:attribute>
          </xsl:if>
          <xsl:copy-of select="@brief"/>
          <xsl:copy-of select="text()"/>
        </xsl:copy>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="import">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="Name"/>
      <xsl:call-template name="Visibility"/>
      <xsl:copy-of select="@param"/>
      <xsl:apply-templates select="export[1]"/>
      <xsl:apply-templates select="body[1]"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="export">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:copy-of select="@source"/>
      <xsl:copy-of select="@name"/>
      <xsl:apply-templates select="reference | interface"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="reference">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="ID"/>
      <xsl:call-template name="Name"/>
      <xsl:choose>
        <xsl:when test="not(@type) or string-length(@type)=0">
          <xsl:attribute name="type">class</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@type"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:choose>
        <xsl:when test="@type='typedef' and (not(@class) or string-length(@class)=0)">
          <xsl:attribute name="class">Unknown</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@class"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:copy-of select="@external"/>
      <xsl:copy-of select="@package"/>
      <xsl:copy-of select="@container"/>
      <xsl:apply-templates select="collaboration"/>
      <xsl:apply-templates select="enumvalue"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="interface">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="ID"/>
      <xsl:call-template name="Name"/>
      <xsl:choose>
        <xsl:when test="not(@root) or string-length(@root)=0">
          <xsl:attribute name="root">no</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@root"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:copy-of select="@package"/>
      <xsl:apply-templates select="collaboration"/>
      <xsl:apply-templates select="property"/>
      <xsl:apply-templates select="method"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="body">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:choose>
        <xsl:when test="not(@type) or string-length(@type)=0">
          <xsl:attribute name="type">method</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@type"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:copy-of select="line"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="class">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="ID"/>
      <xsl:call-template name="Name"/>
      <xsl:call-template name="Visibility"/>
      <xsl:call-template name="Implementation"/>
      <xsl:choose>
        <xsl:when test="not(@inline) or string-length(@inline)=0">
          <xsl:attribute name="inline">none</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@inline"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:choose>
        <xsl:when test="not(@constructor) or string-length(@constructor)=0">
          <xsl:attribute name="constructor">
            <xsl:choose>
              <xsl:when test="@implementation='abstract'">no</xsl:when>
              <xsl:otherwise>public</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@constructor"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:choose>
        <xsl:when test="not(@destructor) or string-length(@destructor)=0">
          <xsl:attribute name="destructor">
            <xsl:choose>
              <xsl:when test="@implementation='abstract'">no</xsl:when>
              <xsl:otherwise>public</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@destructor"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:copy-of select="@behaviour"/>
      <xsl:apply-templates select="model[position()&lt;3]"/>
      <xsl:apply-templates select="inherited"/>
      <xsl:apply-templates select="dependency"/>
      <xsl:apply-templates select="collaboration"/>
      <xsl:apply-templates select="comment[1]"/>
      <xsl:apply-templates select="import"/>
      <xsl:apply-templates select="typedef"/>
      <xsl:apply-templates select="property"/>
      <xsl:apply-templates select="method"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="package">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="ID"/>
      <xsl:call-template name="Name"/>
      <xsl:copy-of select="@folder"/>
      <xsl:apply-templates select="comment[1]"/>
      <xsl:apply-templates select="import"/>
      <xsl:apply-templates select="typedef"/>
      <xsl:apply-templates select="class"/>
      <xsl:apply-templates select="package"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="relationship">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="ID"/>
      <xsl:call-template name="Action"/>
      <xsl:if test="not(@type)">
        <xsl:attribute name="type">assembl</xsl:attribute>
      </xsl:if>
      <xsl:copy-of select="@type"/>
      <xsl:apply-templates select="father[1]"/>
      <xsl:apply-templates select="child[1]"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="inherited">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="IDREF"/>
      <xsl:call-template name="RangePublic"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="collaboration">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="IDREF"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="dependency">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="IDREF"/>
      <xsl:call-template name="Action"/>
      <xsl:if test="not(@type)">
        <xsl:attribute name="type">interface</xsl:attribute>
      </xsl:if>
      <xsl:copy-of select="@type"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="father">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="IDREF"/>
      <xsl:call-template name="Name"/>
      <xsl:call-template name="Member"/>
      <xsl:call-template name="Cardinal"/>
      <xsl:call-template name="Level"/>
      <xsl:call-template name="RangeNo"/>
      <xsl:call-template name="Relation"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="child">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="IDREF"/>
      <xsl:call-template name="Name"/>
      <xsl:call-template name="Member"/>
      <xsl:call-template name="Cardinal"/>
      <xsl:call-template name="Level"/>
      <xsl:call-template name="RangePublic"/>
      <xsl:call-template name="Relation"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="typedef">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="Name"/>
      <xsl:call-template name="ID"/>
      <xsl:call-template name="Typedef"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="return">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="Typedef"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Typedef">
    <xsl:text>
</xsl:text>
    <xsl:apply-templates select="type[1]"/>
    <xsl:apply-templates select="variable[1]"/>
    <xsl:apply-templates select="comment[1]"/>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="type">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="DescIdref"/>
      <xsl:copy-of select="@struct"/>
      <xsl:call-template name="Modifier"/>
      <xsl:call-template name="Level"/>
      <xsl:if test="not(@by)">
        <xsl:attribute name="by">0</xsl:attribute>
      </xsl:if>
      <xsl:copy-of select="@by"/>
      <xsl:apply-templates select="enumvalue"/>
      <xsl:apply-templates select="element"/>
      <xsl:apply-templates select="list"/>
      <xsl:apply-templates select="array"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="enumvalue">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="Name"/>
      <xsl:call-template name="ID"/>
      <xsl:copy-of select="@value"/>
      <xsl:copy-of select="text()"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="element">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="Name"/>
      <xsl:call-template name="NumID"/>
      <xsl:call-template name="DescIdref"/>
      <xsl:call-template name="Modifier"/>
      <xsl:call-template name="Level"/>
      <xsl:copy-of select="@size"/>
      <xsl:copy-of select="@sizeref"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="list">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="Level"/>
      <xsl:if test="not(@type) or string-length(@type)=0">
        <xsl:attribute name="type">simple</xsl:attribute>
      </xsl:if>
      <xsl:copy-of select="@type"/>
      <xsl:call-template name="DescIdref"/>
      <xsl:if test="@type='indexed'">
        <xsl:choose>
          <xsl:when test="not(@index-desc) and not(@index-idref)">
            <xsl:attribute name="index-desc">bool</xsl:attribute>
          </xsl:when>
          <xsl:when test="(@index-desc and string-length(@index-desc)=0) or (@index-idref and string-length(@index-idref)=0)">
            <xsl:attribute name="index-desc">bool</xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="@index-desc"/>
            <xsl:copy-of select="@index-idref"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="array">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:choose>
        <xsl:when test="not(@sizeref) and not(@size)">
          <xsl:attribute name="size">10</xsl:attribute>
        </xsl:when>
        <xsl:when test="(@sizeref and string-length(@sizeref)=0) or (@size and string-length(@size)=0)">
          <xsl:attribute name="size">10</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@size"/>
          <xsl:copy-of select="@sizeref"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="variable">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="RangePublic"/>
      <xsl:copy-of select="@value"/>
      <xsl:copy-of select="@valref"/>
      <xsl:copy-of select="@size"/>
      <xsl:copy-of select="@sizeref"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="property">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="NumID"/>
      <xsl:call-template name="Name"/>
      <xsl:copy-of select="@attribute"/>
      <xsl:copy-of select="@overridable"/>
      <xsl:call-template name="Overrides"/>
      <xsl:copy-of select="@behaviour"/>
      <xsl:copy-of select="@access-value"/>
      <xsl:call-template name="Member"/>
      <xsl:call-template name="Typedef"/>
      <xsl:apply-templates select="get[1]"/>
      <xsl:apply-templates select="set[1]"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="param">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="NumID"/>
      <xsl:call-template name="Name"/>
      <xsl:call-template name="Typedef"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="get">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="RangeNo"/>
      <xsl:call-template name="By"/>
      <xsl:call-template name="Modifier"/>
      <xsl:copy-of select="@inline"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="set">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="RangeNo"/>
      <xsl:call-template name="By"/>
      <xsl:copy-of select="@inline"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="model">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="ID"/>
      <xsl:attribute name="name">
        <xsl:choose>
          <xsl:when test="not(@name) or string-length(@name)=0">
            <xsl:if test="position()=1">A</xsl:if>
            <xsl:if test="position()=2">B</xsl:if>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="@name"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template match="method">
    <xsl:text>
</xsl:text>
    <xsl:copy>
      <xsl:call-template name="NumID"/>
      <xsl:copy-of select="@inline"/>
      <xsl:call-template name="Modifier"/>
      <xsl:call-template name="Implementation"/>
      <xsl:copy-of select="@operator"/>
      <xsl:copy-of select="@operator"/>
      <xsl:copy-of select="@behaviour"/>
      <xsl:call-template name="Overrides"/>
      <xsl:call-template name="Member"/>
      <xsl:apply-templates select="exception"/>
      <xsl:choose>
        <xsl:when test="not(@constructor) or string-length(@constructor)=0">
          <xsl:call-template name="Name"/>
          <xsl:attribute name="constructor">no</xsl:attribute>
          <xsl:if test="not(return)">
            <xsl:call-template name="Return"/>
          </xsl:if>
        </xsl:when>
        <xsl:when test="@constructor='no'">
          <xsl:copy-of select="@constructor"/>
          <xsl:call-template name="Name"/>
          <xsl:if test="not(return)">
            <xsl:call-template name="Return"/>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:copy-of select="@constructor"/>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:apply-templates select="return[1]"/>
      <xsl:apply-templates select="comment[1]"/>
      <xsl:apply-templates select="param"/>
    </xsl:copy>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="ID">
    <xsl:attribute name="id">
      <xsl:choose>
        <xsl:when test="not(@id)">
          <xsl:value-of select="generate-id()"/>
        </xsl:when>
        <xsl:when test="string-length(@id)=0">
          <xsl:value-of select="generate-id()"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@id"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="IDREF">
    <xsl:attribute name="idref">
      <xsl:choose>
        <xsl:when test="not(@idref)">
          <xsl:value-of select="//class/@id"/>
        </xsl:when>
        <xsl:when test="string-length(@idref)=0">
          <xsl:value-of select="//class/@id"/>
        </xsl:when>
        <xsl:when test="not(id(@idref))">
          <xsl:value-of select="//class/@id"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@idref"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Overrides">
      <xsl:choose>
        <xsl:when test="not(@overrides)"/>
        <xsl:when test="string-length(@overrides)=0"/>
        <xsl:when test="not(id(@overrides))"/>
        <xsl:otherwise>
          <xsl:copy-of select="@overrides"/>
        </xsl:otherwise>
      </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Name">
    <xsl:attribute name="name">
      <xsl:choose>
        <xsl:when test="not(@name)">Unknown</xsl:when>
        <xsl:when test="string-length(@name)=0">Unknown</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@name"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Member">
    <xsl:attribute name="member">
      <xsl:choose>
        <xsl:when test="not(@member)">object</xsl:when>
        <xsl:when test="string-length(@member)=0">object</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@member"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Level">
    <xsl:attribute name="level">
      <xsl:choose>
        <xsl:when test="not(@level)">0</xsl:when>
        <xsl:when test="string-length(@level)=0">0</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@level"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Visibility">
    <xsl:attribute name="visibility">
      <xsl:choose>
        <xsl:when test="not(@visibility)">package</xsl:when>
        <xsl:when test="string-length(@visibility)=0">package</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@visibility"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Modifier">
    <xsl:attribute name="modifier">
      <xsl:choose>
        <xsl:when test="not(@modifier)">var</xsl:when>
        <xsl:when test="string-length(@modifier)=0">var</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@modifier"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="By">
    <xsl:attribute name="by">
      <xsl:choose>
        <xsl:when test="not(@by)">val</xsl:when>
        <xsl:when test="string-length(@by)=0">val</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@by"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="RangeNo">
    <xsl:attribute name="range">
      <xsl:choose>
        <xsl:when test="not(@range)">no</xsl:when>
        <xsl:when test="string-length(@range)=0">no</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@range"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="RangePublic">
    <xsl:attribute name="range">
      <xsl:choose>
        <xsl:when test="not(@range)">public</xsl:when>
        <xsl:when test="string-length(@range)=0">public</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@range"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Relation">
    <xsl:choose>
      <xsl:when test="not(@cardinal) or string-length(@cardinal)=0">
        <get range="no" by="val" modifier="var"/>
        <set range="no" by="val"/>
      </xsl:when>
      <xsl:when test="(@cardinal='01' or @cardinal='1') and (not(get) or not(set))">
        <get range="no" by="val" modifier="var"/>
        <set range="no" by="val"/>
      </xsl:when>
      <xsl:when test="(@cardinal='0n' or @cardinal='1n') and not(list) and not(array)">
        <array size="10"/>
      </xsl:when>
      <xsl:when test="@cardinal='01' or @cardinal='1'">
        <xsl:apply-templates select="get | set"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="list | array"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Return">
    <return>
      <type desc="void" modifier="var" level="0" by="val"/>
      <variable range="public"/>
      <comment brief="A brief comment">A multi-lines comment</comment>
    </return>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Cardinal">
    <xsl:attribute name="cardinal">
      <xsl:choose>
        <xsl:when test="not(@cardinal)">1</xsl:when>
        <xsl:when test="string-length(@cardinal)=0">1</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@cardinal"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="NumID">
    <xsl:attribute name="num-id">
      <xsl:choose>
        <xsl:when test="not(@num-id)">
          <xsl:value-of select="position()"/>
        </xsl:when>
        <xsl:when test="string-length(@num-id)=0">
          <xsl:value-of select="position()"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@num-id"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Implementation">
    <xsl:attribute name="implementation">
      <xsl:choose>
        <xsl:when test="not(@implementation) or string-length(@implementation)=0">
          simple
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@implementation"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="DescIdref">
    <xsl:choose>
      <xsl:when test="not(@desc) and not(@idref)">
        <xsl:attribute name="desc">int16</xsl:attribute>
      </xsl:when>
      <xsl:when test="(@desc and string-length(@desc)=0) or (@idref and string-length(@idref)=0)">
        <xsl:attribute name="desc">int16</xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy-of select="@desc"/>
        <xsl:if test="@idref">
          <xsl:call-template name="IDREF"/>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
  <xsl:template name="Action">
    <xsl:choose>
      <xsl:when test="not(@action) or string-length(@action)=0">
        <xsl:attribute name="action">Unknown action</xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy-of select="@action"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ======================================================================= -->
</xsl:stylesheet>






























