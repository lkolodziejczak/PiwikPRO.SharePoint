(function () {
    //PROD
    //var assetsUrl = 'https://piwikpro-sharepoint.azureedge.net';
    //var manifestUrl = assetsUrl + '/asset-manifest.json';

    //DEV
    var siteUrl = '/sites/PiwikAdmin';
    var assetsUrl = siteUrl + '/Style Library/PROD';
    var manifestUrl = siteUrl + "/_api/web/GetFileByServerRelativeUrl('" + assetsUrl + "/asset-manifest.json" + "')/$value";

    function ready(fn) {
        if (document.readyState != 'loading') {
            fn();
        } else {
            document.addEventListener('DOMContentLoaded', fn);
        }
    }

    function getJson(url, callback) {
        var request = new XMLHttpRequest();
        request.open('GET', url, true);

        request.onload = function () {
            if (this.status >= 200 && this.status < 400) {
                var data = JSON.parse(this.response);
                callback(data);
            }
        };

        request.send();
    }

    function loadScript(url, callback) {
        var script = document.createElement('script');
        script.onload = function () {
            callback();
        };
        script.type = 'text/javascript';
        script.src = url;
        script.async = true;
        document.head.appendChild(script);
    }

    function loadCss(url) {
        var link = document.createElement('link');
        link.rel = 'stylesheet';
        link.type = 'text/css';
        link.href = url;
        document.head.appendChild(link);
    }

    function loadScripts(urls) {
        if (urls.length === 0)
            return;
        loadScript(urls.shift(), function () { loadScripts(urls); });
    }

    ready(function () {
        getJson(manifestUrl, function (manifest) {
            var jsUrls = [];

            for (var i = 0; i < manifest.entrypoints.length; i++) {
                var entryPoint = manifest.entrypoints[i];
                if (/\.js$/.test(entryPoint)) {
                    jsUrls.push(assetsUrl + "/" + entryPoint);
                }
                else if (/\.css$/.test(entryPoint)) {
                    loadCss(assetsUrl + "/" + entryPoint);
                }
            }

            loadScripts(jsUrls);
        });
    });
})();