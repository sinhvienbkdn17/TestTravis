define(["jquery"],function(c){return{init:function(s,n,e,t){c.ajax({dataType:"script",cache:!0,url:"modules/Connect.UserAccess/scripts/bootstrap.min.js",success:function(){c.ajax({dataType:"script",cache:!0,url:"modules/Connect.UserAccess/scripts/bundles/connect-useraccess.js",success:function(){window.connect.useraccess.init(n,e)}})}}),"function"==typeof t&&t()},load:function(c,s){window.connect.useraccess.load(c),"function"==typeof s&&s()}}});