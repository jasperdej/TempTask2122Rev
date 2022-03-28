Handlebars.registerHelper("convertToJSON", function convertToJSON(data, options) 
{
    return options.fn(JSON.parse(data));
});
Handlebars.registerPartial("boardSection", Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, alias3="function", alias4=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<section id=\""
    + alias4(((helper = (helper = lookupProperty(helpers,"x") || (depth0 != null ? lookupProperty(depth0,"x") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"x","hash":{},"data":data,"loc":{"start":{"line":1,"column":13},"end":{"line":1,"column":18}}}) : helper)))
    + ","
    + alias4(((helper = (helper = lookupProperty(helpers,"y") || (depth0 != null ? lookupProperty(depth0,"y") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"y","hash":{},"data":data,"loc":{"start":{"line":1,"column":19},"end":{"line":1,"column":24}}}) : helper)))
    + "\" class=\"grid\"></section>";
},"useData":true}));
Handlebars.registerPartial("fiche", Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var helper, alias1=depth0 != null ? depth0 : (container.nullContext || {}), alias2=container.hooks.helperMissing, alias3="function", alias4=container.escapeExpression, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<section id=\""
    + alias4(((helper = (helper = lookupProperty(helpers,"x") || (depth0 != null ? lookupProperty(depth0,"x") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"x","hash":{},"data":data,"loc":{"start":{"line":1,"column":13},"end":{"line":1,"column":18}}}) : helper)))
    + ","
    + alias4(((helper = (helper = lookupProperty(helpers,"y") || (depth0 != null ? lookupProperty(depth0,"y") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"y","hash":{},"data":data,"loc":{"start":{"line":1,"column":19},"end":{"line":1,"column":24}}}) : helper)))
    + "fiche\" class=\"fiche fiche--"
    + alias4(((helper = (helper = lookupProperty(helpers,"colour") || (depth0 != null ? lookupProperty(depth0,"colour") : depth0)) != null ? helper : alias2),(typeof helper === alias3 ? helper.call(alias1,{"name":"colour","hash":{},"data":data,"loc":{"start":{"line":1,"column":51},"end":{"line":1,"column":61}}}) : helper)))
    + "\" onclick=\"Game.Reversi.showFiche(this.id)\"></section>";
},"useData":true}));
this["spa_templates"] = this["spa_templates"] || {};
this["spa_templates"]["templates"] = this["spa_templates"]["templates"] || {};
this["spa_templates"]["templates"]["API"] = this["spa_templates"]["templates"]["API"] || {};
this["spa_templates"]["templates"]["API"]["api"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    return "<script>\r\n    let returnstring = Game.API.GetData('getBord/spelToken');\r\n    \r\n</script>";
},"useData":true});
this["spa_templates"]["templates"]["feedbackWidget"] = this["spa_templates"]["templates"]["feedbackWidget"] || {};
this["spa_templates"]["templates"]["feedbackWidget"]["body"] = Handlebars.template({"1":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return ((stack1 = lookupProperty(helpers,"each").call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? lookupProperty(depth0,"board") : depth0),{"name":"each","hash":{},"fn":container.program(2, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":4,"column":4},"end":{"line":17,"column":13}}})) != null ? stack1 : "");
},"2":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return ((stack1 = lookupProperty(helpers,"each").call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? lookupProperty(depth0,"row") : depth0),{"name":"each","hash":{},"fn":container.program(3, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":5,"column":8},"end":{"line":16,"column":17}}})) != null ? stack1 : "");
},"3":function(container,depth0,helpers,partials,data) {
    var stack1, alias1=depth0 != null ? depth0 : (container.nullContext || {}), lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "            <section class=\"grid--wrapper\">\r\n"
    + ((stack1 = lookupProperty(helpers,"each").call(alias1,(depth0 != null ? lookupProperty(depth0,"grid") : depth0),{"name":"each","hash":{},"fn":container.program(4, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":7,"column":12},"end":{"line":9,"column":21}}})) != null ? stack1 : "")
    + "            </section>\r\n            <section class=\"fiche--wrapper\">\r\n"
    + ((stack1 = lookupProperty(helpers,"each").call(alias1,(depth0 != null ? lookupProperty(depth0,"fiche") : depth0),{"name":"each","hash":{},"fn":container.program(6, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":12,"column":12},"end":{"line":14,"column":21}}})) != null ? stack1 : "")
    + "            </section>\r\n";
},"4":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return ((stack1 = container.invokePartial(lookupProperty(partials,"boardSection"),depth0,{"name":"boardSection","hash":{"y":(depth0 != null ? lookupProperty(depth0,"y") : depth0),"x":(depth0 != null ? lookupProperty(depth0,"x") : depth0)},"data":data,"indent":"                ","helpers":helpers,"partials":partials,"decorators":container.decorators})) != null ? stack1 : "");
},"6":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return ((stack1 = container.invokePartial(lookupProperty(partials,"fiche"),depth0,{"name":"fiche","hash":{"colour":(depth0 != null ? lookupProperty(depth0,"ficheColour") : depth0),"y":(depth0 != null ? lookupProperty(depth0,"y") : depth0),"x":(depth0 != null ? lookupProperty(depth0,"x") : depth0)},"data":data,"indent":"                ","helpers":helpers,"partials":partials,"decorators":container.decorators})) != null ? stack1 : "");
},"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    var stack1, lookupProperty = container.lookupProperty || function(parent, propertyName) {
        if (Object.prototype.hasOwnProperty.call(parent, propertyName)) {
          return parent[propertyName];
        }
        return undefined
    };

  return "<p>help5</p>\r\n\r\n"
    + ((stack1 = (lookupProperty(helpers,"convertToJSON")||(depth0 && lookupProperty(depth0,"convertToJSON"))||container.hooks.helperMissing).call(depth0 != null ? depth0 : (container.nullContext || {}),(depth0 != null ? lookupProperty(depth0,"boardOuter") : depth0),{"name":"convertToJSON","hash":{},"fn":container.program(1, data, 0),"inverse":container.noop,"data":data,"loc":{"start":{"line":3,"column":0},"end":{"line":18,"column":18}}})) != null ? stack1 : "");
},"usePartial":true,"useData":true});
this["spa_templates"]["templates"]["Stats"] = this["spa_templates"]["templates"]["Stats"] || {};
this["spa_templates"]["templates"]["Stats"]["stats"] = Handlebars.template({"compiler":[8,">= 4.3.0"],"main":function(container,depth0,helpers,partials,data) {
    return "";
},"useData":true});