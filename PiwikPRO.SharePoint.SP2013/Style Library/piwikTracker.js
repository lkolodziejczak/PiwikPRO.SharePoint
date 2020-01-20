// Piwik tracker script: @trackerFilePath
// Generation date and time: @generationDateTime
var PiwikScriptVersion = "1.0.0.49";
var PiwikTrackerInstance = null;
var PropertyBagsLoaded = false;
var currentUserLoginHashedOrNot;
var currentUserNameHashedOrNot;
var PiwikImageLogoSrc = 'Style%20Library/js/piwikpro_logo_dark.jpg';

// event path polyfill
function getEventPath(event) {
    var path = (event.composedPath && event.composedPath()) || event.path,
        target = event.target;

    if (path != null) {
        // Safari doesn't include Window, but it should.
        return (path.indexOf(window) < 0) ? path.concat(window) : path;
    }

    if (target === window) {
        return [window];
    }

    function getParents(node, memo) {
        memo = memo || [];
        var parentNode = node.parentNode;

        if (!parentNode) {
            return memo;
        }
        else {
            return getParents(parentNode, memo.concat(parentNode));
        }
    }

    return [target].concat(getParents(target), window);
}

function RunPiwikTracker() {

    try {
        var piwikSiteUUId = this.PropertyBagSettingsSet.piwik_metasitenamestored;

        if (piwikSiteUUId !== '') {
            PiwikTrackerInstance = new PiwikTracker();
            PiwikTrackerInstance.WaitForPageLoadedAndTrack();
            //PiwikTrackerInstance.ValidateInstallation();
        }
    } catch (ex) {
        console.log(ex);
    }
}

function EmbedTagManagerScript() {
    //Start Piwik PRO Tag Manager code
    //<script type="text/javascript">
    (function (window, document, script, dataLayer, id) {

        function stgCreateCookie(a, b, c) {
            var d = ""; if (c) {
                var e = new Date; e.setTime(e.getTime() + 24 * c * 60 * 60 * 1e3), d = "; expires=" + e.toUTCString()
            } document.cookie = a + "=" + b + d + "; path=/"
        }

        var isStgDebug = (window.location.href.match("stg_debug") || window.document.cookie.match("stg_debug")) && !window.location.href.match("stg_disable_debug");

        stgCreateCookie("stg_debug", isStgDebug ? 1 : "", isStgDebug ? 14 : -1);

        window[dataLayer] = window[dataLayer] || [], window[dataLayer].push({ start: (new Date).getTime(), event: "stg.start" });

        var scripts = document.getElementsByTagName(script)[0], tags = document.createElement(script), dl = "dataLayer" !== dataLayer ? "?dataLayer=" + dataLayer : ""; tags.async = !0, tags.src = this.PropertyBagSettingsSet.piwik_containersurl + id + ".js" + dl, isStgDebug && (tags.src = tags.src + "?stg_debug"), scripts.parentNode.insertBefore(tags, scripts);

        !function (a, n, i, t) {
            a[n] = a[n] || {};
            for (var c = 0; c < i.length; c++)
                !function (i) {
                    a[n][i] = a[n][i] || {}, a[n][i].api = a[n][i].api || function () {
                        var a = [].slice.call(arguments, 0), t = a; "string" === typeof a[0] && (t = { event: n + "." + i + ":" + a[0], parameters: [].slice.call(arguments, 1) }), window[dataLayer].push(t)
                    }
                }(i[c])
        }(window, "ppms", ["tm", "cp", "cm"]);

    })(window, document, 'script', 'dataLayer', this.PropertyBagSettingsSet.piwik_metasitenamestored);
}

function PiwikTracker() {

    this.PIWIK_SEARCH_PAGE = 'results.aspx';
    this.PIWIK_SEARCH_PAGE_MODERN = 'search.aspx';
    this.PIWIK_ADD_DOCUMENT_PAGE = 'upload.aspx';
    this.PIWIK_ADD_PAGE = 'createwebpage.aspx';
    this.SESSION_PAGE_ADDED_KEY = 'pageAdded';

    this.MUI_DragAndDropEventAttached = false;

    var TrackerAction = {
        SiteSearch: 0,
        DocumentUpload: 1,
        PageAdded: 2,
        PageEdited: 3,
        DocumentLibrarySearch: 4
    }

    // Global settings
    this.GlobalSettings = {
        doTrack: true
    };

    // Tracker settings
    this.SiteTrackerSettings = {
        //    piwikUrl: '@piwikUrl',
        //    piwikSiteUUId: '@UUID',
        //    useGoalDocumentAdded: @useGoalDocumentAdded,
        //goalDocumentAddedId: @goalDocumentAddedId,
        //useGoalPageAdded: @useGoalPageAdded,
        //goalPageAddedId: @goalPageAddedId,
        //useGoalPageEdited: @useGoalPageEdited,
        //goalPageEditedId: @goalPageEditedId,
        //sendUserEncoded: @sendUserEncoded,
        //sendUserExtendedInfo: @sendUserExtendedInfo,
        //sendUsername: @sendUsername,
        //sendOffice: @sendOffice,
        //sendJobTitle: @sendJobTitle,
        //sendDepartment: @sendDepartment,
        //enforceSSL: @enforceSSL

        piwikUrl: PropertyBagSettingsSet.piwik_serviceurl,
        piwikSiteUUId: PropertyBagSettingsSet.piwik_metasitenamestored,
        useGoalDocumentAdded: PropertyBagSettingsSet.piwik_usegoaldocumentadded,
        goalDocumentAddedId: PropertyBagSettingsSet.piwik_goaldocumentaddedid,
        useGoalPageAdded: PropertyBagSettingsSet.piwik_usegoalpageadded,
        goalPageAddedId: PropertyBagSettingsSet.piwik_goalpageaddedid,
        useGoalPageEdited: PropertyBagSettingsSet.piwik_usegoalpageedited,
        goalPageEditedId: PropertyBagSettingsSet.piwik_goalpageeditedid,
        sendUserEncoded: PropertyBagSettingsSet.piwik_senduserencoded,
        sendUserExtendedInfo: PropertyBagSettingsSet.piwik_senduserextendedinfo,
        sendUsername: PropertyBagSettingsSet.piwik_sendusername,
        sendOffice: PropertyBagSettingsSet.piwik_sendoffice,
        sendJobTitle: PropertyBagSettingsSet.piwik_sendjobtitle,
        sendDepartment: PropertyBagSettingsSet.piwik_senddepartment,
        enforceSSL: PropertyBagSettingsSet.piwik_enforcessl,
        containersUrl: PropertyBagSettingsSet.piwik_containersurl
    };

    // Instance context settings / flow settings
    this.ContextSettings = {
        modalDialogUrl: null,
        modalDialogDoc: null,
        personProperties: null,
        webProperties: null,
        currentUser: null,
        currentUserLogin: '',
        currentUserName: '',
        office: '',
        jobTitle: '',
        department: '',
    }

    this.SearchContextSettings = {
        searchResultsCount: null,
        searchCategories: [],
        searchTerm: null
    }

    // Private methods
    this.showStatus = function (message, color, timeout) {

        var self = this

        if (self.statusId) {
            SP.UI.Status.removeStatus(self.statusId);
        }

        self.statusId = SP.UI.Status.addStatus(message, '');

        SP.UI.Status.setStatusPriColor(self.statusId, color || 'red');
        setTimeout(function () { SP.UI.Status.removeStatus(self.statusId); }, timeout || 5000);
    }

    this.showNotification = function (message, bSticky) {

        var notification;
        ExecuteOrDelayUntilScriptLoaded(function () {
            notification = SP.UI.Notify.addNotification(message, bSticky);
        }, 'sp.ui.dialog.js');
    }

    this.isModernUI = function () {
        return document.getElementById("spoAppComponent") != null || document.querySelector('div[class^="pageLayout_"]') != null;
    }

    this.getDocumentTitleForTracker = function () {
        var pageTitle = document.title;
        var slash = '/';
        var slashRegularExpression = new RegExp(slash, 'g');
        pageTitle = pageTitle.replace(slashRegularExpression, '%2F');
        var dash = ' - ';
        var dashRegularExpression = new RegExp(dash, 'g');
        pageTitle = pageTitle.replace(dashRegularExpression, '/');

        return pageTitle;
    }

    this.listenForElementShowUp = function (selector, triesCount, callback) {
        var intervalTimeInMiliseconds = 500;
        var currentTry = 0;
        var intervalId = setInterval(function () {
            currentTry++;

            var searchedElement = document.querySelector(selector);

            if (searchedElement) {
                clearInterval(intervalId);
                if (callback) {
                    callback(searchedElement);
                }
            }

            if (currentTry === triesCount) {
                clearInterval(intervalId);
            }
        }, intervalTimeInMiliseconds);
    }

    this.waitUntilElementDisappears = function (selector, callback) {
        var intervalId = setInterval(function () {
            var element = document.querySelector(selector);
            if (!element && callback) {
                clearInterval(intervalId);
                callback();
            }
        }, 500);
    }

    this.waitUntilMUIQuickSearchResultsAreRendered = function (callback) {
        function getResultsCount() {
            return document.querySelectorAll('div[role="row"][data-automationid="DetailsRow"].ms-DetailsRow').length;
        }

        var resultsCount = -1;
        var intervalId = setInterval(function () {
            var resultsCountAfterInterval = getResultsCount();

            if (resultsCount === resultsCountAfterInterval) {
                clearInterval(intervalId);
                if (callback) {
                    callback();
                }
            } else {
                resultsCount = resultsCountAfterInterval;
            }
        }, 1000);
    }

    this.attachDocumentUploadHandler = function (element, setDetectedFlag) {
        if (!element) {
            return;
        }

        if (setDetectedFlag === undefined || setDetectedFlag === null || (setDetectedFlag !== true && setDetectedFlag !== false)) {
            setDetectedFlag = false;
        }

        var elementOnClickFunction = element.getAttribute('onclick');
        if (!elementOnClickFunction) {
            elementOnClickFunction = "";
        }

        if (elementOnClickFunction.indexOf('PiwikTrackerInstance.OnDocUpload') < 0) {
            element.setAttribute('onclick', 'PiwikTrackerInstance.OnDocUpload(' + setDetectedFlag + ');' + elementOnClickFunction + ';');
        }
    }

    this.listenForLocationHrefAndDocumentTitleChange = function (callback) {
        var self = this;
        var maxTriesCount = 10;
        var currentTry = 0;
        var initialLocationHref = window.location.href;
        var initialDocumentTitle = self.getDocumentTitleForTracker();

        function stopListeningAndCallback(intervalToStopId) {
            clearInterval(intervalToStopId);

            if (callback) {
                callback(initialLocationHref);
            }
        }

        var intervalId = setInterval(function () {
            currentTry++;
            var currentDocumentTitle = self.getDocumentTitleForTracker();

            if (window.location.href !== initialLocationHref) {
                if (currentDocumentTitle !== initialDocumentTitle || currentTry === maxTriesCount) {
                    stopListeningAndCallback(intervalId);
                }
            } else if (currentTry === maxTriesCount) {
                clearInterval(intervalId);
            }
        }, 500);
    }

    // Start
    PiwikTracker.prototype.WaitForPageLoadedAndTrack = function () {

        if (this.GlobalSettings.doTrack == false)
            return;

        if (SP.SOD.executeFunc != "undefined") {
            this.After_ExecuteOrDelayUntilBodyLoaded();
        }
        else {
            ExecuteOrDelayUntilBodyLoaded(PiwikTrackerInstance.After_ExecuteOrDelayUntilBodyLoaded);
        }
    }

    PiwikTracker.prototype.After_ExecuteOrDelayUntilBodyLoaded = function () {
        try {
            SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
                SP.SOD.executeOrDelayUntilScriptLoaded(Function.createDelegate(PiwikTrackerInstance, PiwikTrackerInstance.After_executeOrDelayUntilScriptLoaded), 'sp.js');
            });
        } catch (ex) {
            console.log(ex);
        }
    }

    PiwikTracker.prototype.After_executeOrDelayUntilScriptLoaded = function () {
        if (typeof g_MinimalDownload != 'undefined' && Boolean(g_MinimalDownload)) {
            var startUrl = _spPageContextInfo.siteServerRelativeUrl;
            if (startUrl == '/') {
                startUrl = '';
            }
            if (startUrl.slice(-1) != '/') {
                startUrl += '/';
            }
            RegisterModuleInit(startUrl + '@trackerFilePath', RunPiwikTracker);
        }

        var sod = null;
        if (typeof SP.UserProfiles == 'undefined') {
            if (!SP.SOD.executeOrDelayUntilScriptLoaded(Function.createDelegate(PiwikTrackerInstance, PiwikTrackerInstance.InitializeAndTrack), 'sp.userprofiles.js')) {
                sod = LoadSodByKey('userprofile');
            }

            switch (sod) {
                case 1:
                    PiwikTrackerInstance.InitializeAndTrack();
                    break;
                case 2:
                    SP.SOD.executeFunc('userprofile', 'SP.UserProfiles.PeopleManager', function () { });
                    break;

                default:
                    SP.SOD.executeFunc('userprofile', 'SP.UserProfiles.PeopleManager', function () { });
                    break;
            }
        }
        else {
            PiwikTrackerInstance.InitializeAndTrack();
        }
    }

    PiwikTracker.prototype.InitializeAndTrack = function () {

        var self = this;

        //if (this.SiteTrackerSettings.sendUserEncoded == "true")
        // ReInitializeCryptoJS();

        //this.CheckLicense(self.GetSharePointValues.bind(self));

        if (!_spPageContextInfo.userId || this.RestoreCacheData()) {
            if (this.SiteTrackerSettings.sendUserEncoded == "true") {
                if (this.ContextSettings.currentUserLogin && this.ContextSettings.currentUserName) {
                    if (PropertyBagSettingsSet.piwik_sha3 == "true") {
                        currentUserLoginHashedOrNot = CryptoJS.SHA3(this.ContextSettings.currentUserLogin).toString();
                        currentUserNameHashedOrNot = CryptoJS.SHA3(this.ContextSettings.currentUserName).toString();
                    }
                    else {
                        currentUserLoginHashedOrNot = CryptoJS1.SHA1(this.ContextSettings.currentUserLogin).toString();
                        currentUserNameHashedOrNot = CryptoJS1.SHA1(this.ContextSettings.currentUserName).toString();
                    }
                }
            }
            this.AttachTracker();
            return;
        }

        var clientContext = new SP.ClientContext.get_current();

        this.ContextSettings.web = clientContext.get_site().get_rootWeb();
        clientContext.load(this.ContextSettings.web);

        var isUserProfileLoaded = typeof SP.UserProfiles != 'undefined';
        if (isUserProfileLoaded) {
            var peopleManager = new SP.UserProfiles.PeopleManager(clientContext);
            this.ContextSettings.personProperties = peopleManager.getMyProperties();
            clientContext.load(this.ContextSettings.personProperties);
        }

        this.ContextSettings.currentUser = this.ContextSettings.web.get_currentUser();
        clientContext.load(this.ContextSettings.currentUser);

        this.ContextSettings.webProperties = this.ContextSettings.web.get_allProperties();
        clientContext.load(this.ContextSettings.webProperties);

        clientContext.executeQueryAsync(
            Function.createDelegate(this, !isUserProfileLoaded ? this.GetUserProfileDetails : this.LoadedSharePointValues),
            Function.createDelegate(this, this.onQueryFailed));
    }

    PiwikTracker.prototype.GetSharePointValues = function () {

        // anonymous user
        if (!_spPageContextInfo.userId || this.RestoreCacheData()) {
            this.AttachTracker();
            return;
        }

        var clientContext = new SP.ClientContext.get_current();

        this.ContextSettings.web = clientContext.get_site().get_rootWeb();
        clientContext.load(this.ContextSettings.web);

        var isUserProfileLoaded = typeof SP.UserProfiles != 'undefined';
        if (isUserProfileLoaded) {
            var peopleManager = new SP.UserProfiles.PeopleManager(clientContext);
            this.ContextSettings.personProperties = peopleManager.getMyProperties();
            clientContext.load(this.ContextSettings.personProperties);
        }

        this.ContextSettings.currentUser = this.ContextSettings.web.get_currentUser();
        clientContext.load(this.ContextSettings.currentUser);

        this.ContextSettings.webProperties = this.ContextSettings.web.get_allProperties();
        clientContext.load(this.ContextSettings.webProperties);

        clientContext.executeQueryAsync(
            Function.createDelegate(this, !isUserProfileLoaded ? this.GetUserProfileDetails : this.LoadedSharePointValues),
            Function.createDelegate(this, this.onQueryFailed));
    }

    PiwikTracker.prototype.GetUserProfileDetails = function () {

        var self = this;

        var context = new SP.ClientContext.get_current();
        var relativeWebUrl = context.get_url();
        var siteUrl = window.location.protocol + '//' + window.location.host + relativeWebUrl;

        var userLogin = self.ContextSettings.currentUser.get_loginName();

        var soapEnv =
            '<?xml version=\'1.0\' encoding=\'utf-8\'?> \
                <soap:Envelope xmlns:xsi=\'http://www.w3.org/2001/XMLSchema-instance\' xmlns:xsd=\'http://www.w3.org/2001/XMLSchema\' xmlns:soap=\'http://schemas.xmlsoap.org/soap/envelope/\'> \
                <soap:Body> \
                <GetUserProfileByName xmlns=\'http://microsoft.com/webservices/SharePointPortalServer/UserProfileService\'> \
                  <AccountName>' + userLogin + '</AccountName> \
                </GetUserProfileByName> \
              </soap:Body> \
            </soap:Envelope>';


        var xhr = new XMLHttpRequest();
        xhr.open('POST', siteUrl + '/_vti_bin/userprofileservice.asmx', true);
        xhr.setRequestHeader('Content-type', 'text/xml; charset=\'utf-8\'');
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var nodes = xhr.responseXML.getElementsByTagName('PropertyData')
                for (var i = 0; i < nodes.length; i++) {

                    var node = nodes[i].getElementsByTagName('Name')[0] || {};
                    var nodeText = node.textContent || node.text || node.innerText || node.innerHTML || '';

                    var valueNode = nodes[i].getElementsByTagName('Value')[0] || {};
                    var valueNodeText = valueNode.textContent || valueNode.text || valueNode.innerText || valueNode.innerHTML || '';

                    switch (nodeText) {
                        case 'Department':
                            self.ContextSettings.department = valueNodeText;
                            break;
                        case 'SPS-JobTitle':
                            self.ContextSettings.jobTitle = valueNodeText;
                            break;
                        case 'Office':
                            self.ContextSettings.office = valueNodeText;
                            break;
                    }
                }
            }

            if (xhr.readyState == 4) {
                self.LoadedSharePointValues();
            }
        }

        xhr.send(soapEnv);
    }

    PiwikTracker.prototype.onQueryFailed = function (sender, args) {
        this.showNotification('Request failed. Error: ' + args.get_message() + 'StackTrace: ' + args.get_stackTrace(), false);
    }

    PiwikTracker.prototype.LoadedSharePointValues = function () {

        // Get user logon name
        this.ContextSettings.currentUserLogin = this.ContextSettings.currentUser.get_loginName();
        var markerPosition = this.ContextSettings.currentUserLogin.lastIndexOf('|');
        if (markerPosition > 0)
            this.ContextSettings.currentUserLogin = this.ContextSettings.currentUserLogin.substring(markerPosition + 1, this.ContextSettings.currentUserLogin.length);

        // Get user display name
        this.ContextSettings.currentUserName = this.ContextSettings.currentUser.get_title();

        // Use of user profile service
        if (this.ContextSettings.personProperties != null) {
            // Get user display name
            this.ContextSettings.currentUserName = this.ContextSettings.personProperties.get_displayName();

            // Get user office
            this.ContextSettings.office = this.ContextSettings.personProperties.get_userProfileProperties()['Office'];

            // Get user job title
            this.ContextSettings.jobTitle = this.ContextSettings.personProperties.get_userProfileProperties()['SPS-JobTitle'];

            // Get user department
            this.ContextSettings.department = this.ContextSettings.personProperties.get_userProfileProperties()['Department'];

        }

        // Encode user logon name and display name if needed
        if (this.SiteTrackerSettings.sendUserEncoded == "true") {
            currentUserLoginHashedOrNot = CryptoJS.SHA3(this.ContextSettings.currentUserLogin).toString();
            currentUserNameHashedOrNot = CryptoJS.SHA3(this.ContextSettings.currentUserName).toString();
        }

        this.CacheData();

        this.AttachTracker();
    }

    PiwikTracker.prototype.CacheData = function () {

        var cacheData = {
            currentUserLogin: this.ContextSettings.currentUserLogin,
            currentUserName: this.ContextSettings.currentUserName,
            office: this.ContextSettings.office,
            jobTitle: this.ContextSettings.jobTitle,
            department: this.ContextSettings.department
        }
        var cacheKey = "PiwikPRO_" + _spPageContextInfo.systemUserKey;

        try {
            if (typeof (sessionStorage) !== 'undefined') {
                sessionStorage.setItem(cacheKey, JSON.stringify(cacheData));
            }
        }
        catch (err) {
            conole.log("Piwik PRO - failed to store data in cache", ex);
        }
    }

    PiwikTracker.prototype.RestoreCacheData = function cacheData() {

        var cacheKey = "PiwikPRO_" + _spPageContextInfo.systemUserKey;

        try {
            if (typeof (sessionStorage) !== 'undefined' && sessionStorage.getItem(cacheKey)) {
                var data = JSON.parse(sessionStorage.getItem(cacheKey));

                for (var key in data) {
                    if (data.hasOwnProperty(key)) {
                        this.ContextSettings[key] = data[key];
                    }
                }

                return true;
            }
        }
        catch (err) {
            conole.log("Piwik PRO - failed to restore data from cache", ex);
            return false;
        }

        return false;
    }

    PiwikTracker.prototype.AttachTracker = function () {

        // Override UI and track page
        this.OverrideUIActions();
        this.Track();
    }

    PiwikTracker.prototype.OverrideUIActions = function () {
        // Check for dialogs
        try {
            this.ContextSettings.modalDialogUrl = SP.UI.ModalDialog.get_childDialog().get_url();
            this.ContextSettings.modalDialogDoc = SP.UI.ModalDialog.get_childDialog().$0_0.contentDocument;
        }
        catch (err) { this.ContextSettings.modalDialogDoc = document; }

        // Is on classic search page
        if (document.URL.toLowerCase().indexOf(this.PIWIK_SEARCH_PAGE) > 0) {
            this.OverrideClassicSearch();
        }

        // Search on classic document library view / webpart
        this.OverrideClassicDocLibSearch();

        this.OverrideModernSearch();
        this.HookIntoModernSideNav();

        // Is on document add page
        if (document.URL.toLowerCase().indexOf(this.PIWIK_ADD_DOCUMENT_PAGE) > 0) {
            this.OverrideDocUpload();
        }

        // Is on page add
        if (document.URL.toLowerCase().indexOf(this.PIWIK_ADD_PAGE) > 0) {
            this.OverridePageAdding();
        }

        // New document on SharePointOnline
        this.OverrideDocAdding();

        // Upload document on SPOnline
        this.OverrideDocUploadSPO();
        this.OverrideDocUploadClassic();

        if (this.isModernUI()) {
            this.OverrideDocUploadModernExperience();
            this.HookIntoModernQuickSearch();
        }

        // Add document on SPOnline library
        this.OverrideDocAddingSPO();

        // Page edit
        this.OverridePageEditing();

        // New Page on modern HomePage
        this.OverrideModernSiteAdding();
        // modern publishing button support, part of edit/add page stuff
        this.OverrideModernPublishButton();

        // Save and close on Modern New Site
        this.OverrideSiteSaveAndCloseSPO();

        // Open edit page on modern sites on SPOnline
        this.OverrideModernSiteEditSPO();

        // Download links
        this.OverrideDownload();
    }

    PiwikTracker.prototype.OverrideClassicSearch = function () {
        SetObserverForClassicSearchResultsPage();
    }

    PiwikTracker.prototype.OverrideClassicDocLibSearch = function () {
        if (!this.isModernUI()) {

            ExecuteOrDelayUntilScriptLoaded(function () {
                var tmpRefreshViewFunction = window.inplview.HandleRefreshViewByContext;
                var overrideHandleRefreshViewByContext = function (ctxParam, bClearPagingParam) {
                    var searchTerm = ctxParam.searchTerm;
                    tmpRefreshViewFunction.apply(this, arguments);

                    if (searchTerm != null) {
                        var searchResultsComponent =
                            document.querySelector("#script" + ctxParam.wpq + " > table.ms-listviewtable");

                        if (searchResultsComponent == null) {
                            console.log('Piwik PRO: Unable to find search results control');
                            return;
                        }

                        var searchResultsObserver = new MutationObserver(function (mutations) {
                            if (mutations[0].type === "childList") {
                                PiwikTrackerInstance.SearchContextSettings.searchResultsCount = searchResultsComponent.querySelectorAll('tbody > tr').length;
                                PiwikTrackerInstance.SearchContextSettings.searchCategories = [];
                                PiwikTrackerInstance.SearchContextSettings.searchTerm = searchTerm;

                                PiwikTrackerInstance.Track(TrackerAction.DocumentLibrarySearch);
                                this.disconnect();
                            }
                        });

                        var config = { childList: true, subtree: true }
                        searchResultsObserver.observe(searchResultsComponent, config);
                    }
                }

                window.inplview.HandleRefreshViewByContext = overrideHandleRefreshViewByContext;
            }, 'inplview.js');
        }
    }

    PiwikTracker.prototype.OverrideModernSearch = function () {
        var searchButtonRef = null;

        setInterval(function () {
            var searchModernPageForm = document.querySelector('form[role="search"]');
            if (searchModernPageForm != null) {
                var lastChild = searchModernPageForm.children[searchModernPageForm.children.length - 1];
                if (lastChild.tagName !== "BUTTON") {
                    console && console.log && console.log("OverrideModernSearch - UI have changed, cannot inject");
                    return;
                }

                if (searchButtonRef != lastChild) {
                    lastChild.addEventListener("click", function () {
                        // todo: this could be changed to observer instead of static 2s time; however there are no clear hook points to do so
                        setTimeout(function () {
                            var searchHost = document.getElementsByClassName("ms-searchux")[0];
                            PiwikTrackerInstance.SearchContextSettings.searchResultsCount = searchHost != null ? searchHost.querySelectorAll("article").length : null;
                            PiwikTrackerInstance.OnSearch();
                        }, 2000);
                    });
                    searchButtonRef = lastChild;
                }
            }
        }, 1000);
    }

    PiwikTracker.prototype.HookIntoModernSideNav = function () {
        var self = this;
        function loadSideNavigationHook() {
            var elements = document.querySelectorAll('li.ms-Nav-navItem a.ms-Nav-link:not([data-sidenaveventattached])');
            for (var i = 0; i < elements.length; i++) {
                var navItem = elements[i];

                navItem.setAttribute('data-sidenaveventattached', true);
                navItem.addEventListener('click', function () {
                    self.listenForLocationHrefAndDocumentTitleChange(function (prevHref) {
                        setTimeout(function () {
                            _paq.push(['setReferrerUrl', prevHref]);
                            _paq.push(['setCustomUrl', location.href.toLowerCase()]);
                            _paq.push(['setDocumentTitle', self.getDocumentTitleForTracker()]);
                            _paq.push(['setGenerationTimeMs', 0]);
                            window._paq.push(['trackPageView']);
                            self.OverrideUIActions();
                        }, 0);
                    });
                });
            }
        }

        if (!this.isModernUI()) {
            return;
        }

        loadSideNavigationHook();
    }

    PiwikTracker.prototype.HookIntoModernDownloadButtons = function () {
        var self = this;
        var observer = null;

        function getDownloadLabel() {
            var lpc = window.LivePersonaCardStrings;
            return lpc && lpc.strings['immersiveProfileStrings.immersiveProfileFileSectionStrings.horizontalSectionDownloadAction'] || "Download";
        }

        function downloadListener() {
            var folderRegex = window.location.href.match("&id=([^&]+)");
            var folder = folderRegex != null && folderRegex.length > 2 ? folderRegex[1] : null;

            var subPath = "https://" + window.location.host + (folder || window._spPageContextInfo.listUrl);

            var elements = document.querySelectorAll('div.is-selected div[data-automationid="DetailsRowCell"] .ms-Link');
            for (var i = 0; i < elements.length; i++) {
                var anchor = elements[i];

                var tracker = window._paq;
                var finalPath = subPath + "/" + anchor.innerHTML;
                if (tracker) {
                    tracker.push(['trackLink', finalPath, 'download']);
                } else {
                    console && console.log && console.log("d: %o", finalPath);
                }
            }
        }

        function loadContextualMenuDownloadHook() {
            setInterval(function () {
                if (observer != null) {
                    return;
                }

                var observerHost = document.querySelector('body');
                if (!observerHost) {
                    return;
                }

                observer = new MutationObserver(function () {
                    window.setTimeout(function () {
                        var elements = document.querySelectorAll('li.ms-ContextualMenu-item > button.ms-ContextualMenu-link[name="' + getDownloadLabel() + '"]:not([data-downloadeventattached])');
                        for (var i = 0; i < elements.length; i++) {
                            var menu = elements[i];
                            menu.setAttribute('data-downloadeventattached', true);
                            menu.addEventListener('click', downloadListener);
                        }
                    }, 0);
                });
                observer.observe(observerHost, { childList: true });
            }, 1000);
        }

        function loadTopMenuDownloadHook() {
            setInterval(function () {
                var topMenu = document.querySelectorAll('.ms-OverflowSet-item > button[name="' + getDownloadLabel() + '"]:not([data-downloadeventattached])')[0];

                if (topMenu == null) {
                    return;
                }

                topMenu.setAttribute('data-downloadeventattached', true);
                topMenu.addEventListener('click', downloadListener);
            }, 1000);
        }

        if (!self.isModernUI()) {
            return;
        }

        loadTopMenuDownloadHook();
        loadContextualMenuDownloadHook();
    }

    PiwikTracker.prototype.OnSearch = function () {
        PiwikTrackerInstance.Track(TrackerAction.SiteSearch);
    }

    function SetObserverForClassicSearchResultsPage() {
        if (IsSearchResultsVisible()) {
            CallTrackerForSearch();
        }
        else {
            CreateObserver();
        }

        window.onhashchange = function () {
            CreateObserver();
        }

        function CreateObserver() {
            var searchResultsComponent = document.querySelector('.ms-srch-siteSearchResults') || document.querySelector('.ms-srch-result');

            if (IsEnterpriseSearchCenter()) {
                searchResultsComponent = document.querySelector('.ms-searchCenter-result-main');
            }

            if (searchResultsComponent == null) {
                console.log('Piwki PRO: Unable to find search results control');
                return;
            }

            var searchResultsObserver = new MutationObserver(function (mutations) {
                if (mutations[0].type === "childList") {
                    CallTrackerForSearch();
                    this.disconnect();
                }
            });

            var config = { childList: true, subtree: true }
            searchResultsObserver.observe(searchResultsComponent, config);

            function IsEnterpriseSearchCenter() {
                var currentLocation = window.location.href;

                return currentLocation.indexOf('/results.aspx') > -1 ||
                    currentLocation.indexOf('/peopleresults.aspx') > -1 ||
                    currentLocation.indexOf('/conversationresults.aspx') > -1 ||
                    currentLocation.indexOf('/videoresults.aspx') > -1;
            }
        }

        function CallTrackerForSearch() {
            PiwikTrackerInstance.SearchContextSettings.searchResultsCount = GetResultsCount();
            PiwikTrackerInstance.SearchContextSettings.searchCategories = GetSearchCategoriesFromRefiners();
            PiwikTrackerInstance.Track(TrackerAction.SiteSearch);
        }

        function IsSearchResultsVisible() {
            var resultElement = document.getElementById("Result");
            return resultElement !== undefined && resultElement !== null;
        }

        function GetResultsCount() {
            var resultsCountElement = document.getElementById("ResultCount");
            if (resultsCountElement === null) {
                return 0;
            }

            return parseInt(resultsCountElement.innerText.match(/\d+/)[0]);
        }

        function GetSearchCategoriesFromRefiners() {
            var refinersSections = document.querySelectorAll("a.ms-ref-refinername");
            var searchCategories = [];

            for (var i = 0; i < refinersSections.length; i++) {
                var refinerSection = refinersSections[i];

                var category = {
                    FilterName: '',
                    UsedFilters: []
                };

                if (refinerSection != null) {
                    var categoryName = PiwikTrackerInstance.getElementByIdContains(refinerSection, "RefinerHeading", "div").innerHTML;
                    category.FilterName = categoryName;
                }

                category.UsedFilters = GetUsedRefiners(refinerSection);

                if (category.UsedFilters.length === 0) {
                    continue;
                }

                searchCategories.push(category);
            }

            var parsedCategories = [];

            for (var j = 0; j < searchCategories.length; j++) {
                var searchCategory = searchCategories[j];
                var parsedCategory = searchCategory.FilterName + ": " + searchCategory.UsedFilters.join(', ');
                parsedCategories.push(parsedCategory);
            }

            return parsedCategories;
        }

        function GetUsedRefiners(refinerSection) {
            var usedRefiners = [];

            // for text refiners 
            var usedSection = PiwikTrackerInstance.getElementByIdContains(refinerSection.parentNode, "SelectedSection", "div");
            if (usedSection != undefined) {
                var refinersUsed = usedSection.children;
                for (var i = 0; i < refinersUsed.length; i++) {
                    usedRefiners.push(refinersUsed[i].innerText.trim());
                }
                return usedRefiners;
            }

            // for range(date) refiners
            var rangeSection = PiwikTrackerInstance.getElementByIdContains(refinerSection.parentNode, "RangeLabel", "span");
            if (rangeSection != undefined && rangeSection.classList.contains("ms-ref-filterSel")) {
                usedRefiners.push(rangeSection.innerText.trim());
            }

            return usedRefiners;
        }
    }

    PiwikTracker.prototype.OverrideDocUpload = function () {
        var okbutton = null;

        try {
            okbutton = this.getElementByIdContains(this.ContextSettings.modalDialogDoc, 'btnOK', 'input');
        }
        catch (err) {
            return;
        }

        this.attachDocumentUploadHandler(okbutton, true);
    }

    PiwikTracker.prototype.OverrideDocAdding = function () {
        document.body.addEventListener('click', function (event) {
            var clickedItem = event.target;

            // check if clicked elements is a part of control for SPOnline controls for document creation
            if (clickedItem && clickedItem.id.indexOf('js-newdocWOPI-') === 0 &&
                (clickedItem.tagName === "A" || clickedItem.tagName === "IMG" || clickedItem.tagName === "H3")) {
                PiwikTrackerInstance.OnDocUpload(true);
            }
        });
    }

    PiwikTracker.prototype.OverrideDocUploadSPO = function () {
        document.addEventListener("change", function (event) {
            var element = event.target;
            if (element && element.tagName === "INPUT" &&
                (element.className.indexOf("ContextualMenu-fileInput") > -1 || (element.parentElement != null && element.parentElement.className.indexOf("ContextualMenu-uploadInput") > -1))) {
                // Needed to wait for file to add to SharePoint. Without that there were not adding
                setTimeout(function () {
                    PiwikTrackerInstance.OnDocUpload(true);
                }, 500);
            }
        });
    }

    PiwikTracker.prototype.SetUploadAlreadyDetectedFlag = function () {
        this._docUploadAlreadyDetected = true;
    }

    PiwikTracker.prototype.OverrideDocUploadClassic = function () {
        var self = this;
        var fileUploadedFlag = 5;

        if (window.UploadFinishFunc == null) {
            // not in classic mode
            return;
        }

        // we override built-in fuction, to detect drag & drop uploads as well
        var oldFunction = window.UploadFinishFunc;
        window.UploadFinishFunc = function (element, uploadState) {
            var result = oldFunction(element, uploadState);
            if (self._docUploadAlreadyDetected == null && uploadState && uploadState.files) {
                for (var i = 0; i < uploadState.files.length; i++) {
                    var item = uploadState.files[i];

                    if (item.status === fileUploadedFlag) {
                        self.OnDocUpload();
                    }
                }
            } else {
                self._docUploadAlreadyDetected = null;
            }

            return result;
        }
    }

    PiwikTracker.prototype.HookIntoModernQuickSearch = function () {
        var self = this;
        var searchOccured = false;
        var resultsPageObserver = new MutationObserver(function () {
            setTimeout(function () {
                // is on quick search view?
                if (location.href.match("view=7&q=") == null) {
                    return;
                }

                // wait until loading spinner disappears
                self.waitUntilElementDisappears('.StandaloneList-innerContent > .StandaloneList-loadingSpinner',
                    function () {
                        self.waitUntilMUIQuickSearchResultsAreRendered(function () {
                            if (searchOccured === true) {
                                self.SearchContextSettings.searchResultsCount = document.querySelectorAll('div[role="row"][data-automationid="DetailsRow"].ms-DetailsRow').length;
                                self.OnSearch();
                                searchOccured = false;
                            }
                        });
                    });
            }, 1000);
        });

        var quickSearchPopupObserver = new MutationObserver(function () {
            var quickSearchProgressBar = document.querySelector('.od-SearchResults-progressBox[data-pbmodernquicksearcheventattached="true"]');
            if (quickSearchProgressBar.style.display === "none") {
                var quickList = document.getElementsByClassName("od-SearchResults-resultsBox ms-noList")[0];
                if (quickList == null) {
                    return;
                }

                setTimeout(function () {
                    // -1 to remove "show more results"
                    self.SearchContextSettings.searchResultsCount = quickList.querySelectorAll(".od-SearchResults-result-title").length - 1;
                    if (self.SearchContextSettings.searchResultsCount < 0) {
                        self.SearchContextSettings.searchResultsCount = 0;
                    }

                    self.OnSearch();
                }, 1500);

                this.disconnect();
                quickSearchProgressBar.removeAttribute('data-pbmodernquicksearcheventattached');
            }
        });

        function onShowQuickSearchResults() {
            self.listenForElementShowUp('.StandaloneList-innerContent', 5, function (element) {
                searchOccured = true;
                resultsPageObserver.disconnect();
                resultsPageObserver.observe(element, { childList: true });
            });
        }

        setInterval(function () {
            var input = document.querySelector("input[role='search']:not([data-inputmodernquicksearcheventattached])");
            if (input) {
                input.setAttribute('data-inputmodernquicksearcheventattached', true);
                input.addEventListener("keyup", function (event) {
                    // enter key
                    if (event.keyCode === 13) {
                        onShowQuickSearchResults();
                    } else {
                        var quickSearchProgressBar = document.querySelector('.od-SearchResults-progressBox:not([data-pbmodernquicksearcheventattached])');
                        if (quickSearchProgressBar) {
                            quickSearchProgressBar.setAttribute('data-pbmodernquicksearcheventattached', true);
                            quickSearchPopupObserver.disconnect();
                            quickSearchPopupObserver.observe(quickSearchProgressBar, { childList: true, attributes: true });
                        }
                    }
                });
            }

            // See more results link button
            var showMoreResultsLink = document.querySelector('.od-SearchResults-result-link[role="link"]:not([data-quicksearcheventattached])');
            if (showMoreResultsLink) {
                showMoreResultsLink.setAttribute('data-quicksearcheventattached', true);
                showMoreResultsLink.addEventListener("click", onShowQuickSearchResults);
            }
        }, 1000);
    }

    PiwikTracker.prototype.OverrideDocUploadModernExperience = function () {
        var self = this;

        function onDocumentDrop(event) {
            var hasFile = event.dataTransfer.files.length > 0 || event.dataTransfer.items.length > 0;
            var wasDroppedOnList = getEventPath(event).filter(function (item) {
                return item.classList && item.classList.contains("StandaloneList-innerContent");
            });

            if (!hasFile || !wasDroppedOnList) {
                return;
            }

            PiwikTrackerInstance.OnDocUpload(true);
        }

        setInterval(function () {
            var icon = document.querySelector('i[data-icon-name="upload"]:not([data-uploadeventattached])');
            if (icon == null) {
                return;
            }

            var downloadButton = icon.parentElement.parentElement;
            if (downloadButton == null) {
                console.log("Upload button not found. Modern UI DOM may have changed.");
                return;
            }

            icon.setAttribute('data-uploadeventattached', true);

            downloadButton.addEventListener("click", function () {
                window.setTimeout(function () {
                    var contextMenu = document.getElementById(downloadButton.getAttribute("aria-owns"));
                    if (contextMenu == null) {
                        return;
                    }

                    var contextMenuItems = contextMenu.querySelectorAll("li > button");
                    for (var i = 0; i < contextMenuItems.length; i++) {
                        var contextMenuItem = contextMenuItems[i];
                        self.attachDocumentUploadHandler(contextMenuItem, true);
                    }
                }, 0);
            });
        }, 1000);

        // drag & drop
        if (self.MUI_DragAndDropEventAttached === false) {
            self.MUI_DragAndDropEventAttached = true;
            document.addEventListener("drop", onDocumentDrop, true);
        }
    }

    PiwikTracker.prototype.OverrideDocAddingSPO = function () {
        var self = this;

        var saveEditInterval = setInterval(function () {
            var automationIdAttributeNames = ["createFolderCommand", "createWordCommand", "createExcelCommand", "createPowerPointCommand", "createOneNoteCommand"];

            var contextMenuButtons = document.querySelectorAll("button.ms-ContextualMenu-link");
            for (var i = 0; i < contextMenuButtons.length; i++) {
                var menuButton = contextMenuButtons[i];

                if (menuButton != null) {
                    var automationIdAttribute = menuButton.getAttribute("data-automationid");
                    if (automationIdAttributeNames.indexOf(automationIdAttribute) > -1) {
                        self.attachDocumentUploadHandler(menuButton, true);
                    }
                }
            }
        }, 1000);
    }

    PiwikTracker.prototype.OverrideSiteSaveAndCloseSPO = function () {
        var saveAndCloseSPOInterval = setInterval(function () {
            var saveAndCloseSPOButton = document.querySelectorAll("button[data-automation-id='pageCommandBarSaveButton']")[0];
            if (saveAndCloseSPOButton != null) {
                var saveEditFunc = saveAndCloseSPOButton.getAttribute('onclick');
                if (saveEditFunc == null) {
                    saveEditFunc = "";
                }
                if (saveEditFunc.indexOf('PiwikTrackerInstance.OnPageEditing(true);') < 0) {
                    saveAndCloseSPOButton.setAttribute('onclick', 'PiwikTrackerInstance.OnPageEditing(true);' + saveEditFunc + ';');
                }
            }
        }, 1000);
    }

    PiwikTracker.prototype.OverrideModernSiteEditSPO = function () {
        var modernSiteEditButton = document.querySelectorAll("button[data-automation-id='pageCommandBarEditButton']")[0];
        if (modernSiteEditButton != undefined && modernSiteEditButton != null) {
            var editFunc = modernSiteEditButton.getAttribute('onclick');
            if (editFunc == null) {
                editFunc = "";
            } else {
                editFunc = editFunc + ";";
            }
            if (editFunc.indexOf('PiwikTrackerInstance.OnPageEditing(true);') < 0) {
                modernSiteEditButton.setAttribute('onclick', 'PiwikTrackerInstance.OnPageEditing(true);' + editFunc);
            }
        }
    }

    PiwikTracker.prototype.OverrideModernSiteAdding = function () {
        function getPageName(url) {
            var result = /\/([^.\/]+\.aspx)/.exec(url);
            return result != null && result.length > 1 && result[1];
        }
        var self = this;
        var modernSiteEditButton = document.querySelector('button[data-automation-id="pageCommandBarPublishButton"]');
        if (modernSiteEditButton != null) {
            modernSiteEditButton.addEventListener("click", function () {
                self.listenForLocationHrefAndDocumentTitleChange(function (prevHref) {
                    var prevPageName = getPageName(prevHref);
                    var currentPageName = getPageName(window.location.href);
                    if (prevPageName != currentPageName && prevPageName != null) {
                        PiwikTrackerInstance.OnPageAdding();
                        window.setTimeout(function () {
                            self.OverrideModernPublishButton();
                        }, 1000);
                    }
                });
            });
        }
    }

    PiwikTracker.prototype.OnDocUpload = function (setDetectedFlag) {
        if (setDetectedFlag) {
            PiwikTrackerInstance.SetUploadAlreadyDetectedFlag();
        }

        this.Track(TrackerAction.DocumentUpload);
    }

    PiwikTracker.prototype.OverridePageAdding = function () {
        var okbutton = null;
        try {
            okbutton = this.getElementByIdContains(this.ContextSettings.modalDialogDoc, 'createButton', 'input');
            if (okbutton == undefined) {
                okbutton = this.getElementByIdContains(this.ContextSettings.modalDialogDoc, 'btnCreate', 'input');
            }
        }
        catch (err) {
            return;
        }
        var okinputfunc = okbutton.getAttribute('onclick');
        if (okinputfunc.indexOf('PiwikTrackerInstance.OnPageAdding();') < 0)
            okbutton.setAttribute('onclick', 'PiwikTrackerInstance.OnPageAdding();' + okinputfunc + ';');
    }

    PiwikTracker.prototype.OnPageAdding = function () {
        this.Track(TrackerAction.PageAdded);
    }

    PiwikTracker.prototype.OverridePageEditing = function () {
        try {
            var wikiInEditMode = document.forms[MSOWebPartPageFormName]._wikiPageMode.value;
            if (wikiInEditMode == 'Edit') {
                var saveEditInterval = setInterval(function () {
                    var pageSaveButton = document.getElementById('Ribbon.EditingTools.CPEditTab.EditAndCheckout.SaveEdit-SelectedItem');
                    if (pageSaveButton != null) {
                        var saveEditfunc = pageSaveButton.getAttribute('onclick');
                        if (saveEditfunc.indexOf('PiwikTrackerInstance.OnPageEdited();') < 0) {
                            pageSaveButton.setAttribute('onclick', 'PiwikTrackerInstance.OnPageEditing();' + saveEditfunc + ';');
                            clearInterval(saveEditInterval);
                        }
                    }
                }, 1000);
                var saveOnTopRightInterval = setInterval(function () {
                    var pageSaveOnTopRightButton = document.getElementById('ctl00_PageStateActionButton');
                    if (pageSaveOnTopRightButton != null) {
                        var saveEditfunc = pageSaveOnTopRightButton.getAttribute('onclick');
                        if (saveEditfunc.indexOf('PiwikTrackerInstance.OnPageEdited();') < 0) {
                            pageSaveOnTopRightButton.setAttribute('onclick', 'PiwikTrackerInstance.OnPageEditing();' + saveEditfunc + ';');
                            clearInterval(saveOnTopRightInterval);
                        }
                    }
                }, 1000);
            }
            else {
                var pageButton = document.getElementById('Ribbon.WikiPageTab-title');
                if (pageButton != undefined && pageButton != null) {
                    var editInterval = setInterval(function () {
                        var editButton = document.getElementById('Ribbon.WikiPageTab.EditAndCheckout.SaveEdit-SelectedItem');
                        if (editButton != undefined && editButton != null) {
                            var editFunc = editButton.getAttribute('onclick');
                            if (editFunc.indexOf('PiwikTrackerInstance.OnPageEditButton();') < 0) {
                                editButton.setAttribute('onclick', 'PiwikTrackerInstance.ClearSessionKeyForPageAdded();PiwikTrackerInstance.OnPageEditButton();' + editFunc + ';');
                                clearInterval(editInterval);
                            }
                        }
                    }, 1000);
                }
                var editOnTopRightInterval = setInterval(function () {
                    var editOnTopRight = document.getElementById("ctl00_PageStateActionButton");
                    if (editOnTopRight != null) {
                        var editOnTopRightFunc = editOnTopRight.getAttribute('onclick');
                        if (editOnTopRightFunc.indexOf('PiwikTrackerInstance.OnPageEdited();') < 0) {
                            editOnTopRight.setAttribute('onclick', 'PiwikTrackerInstance.ClearSessionKeyForPageAdded();' + editOnTopRightFunc + ';');
                            clearInterval(editOnTopRightInterval);
                        }
                    }
                }, 1000);
            }
        }
        catch (err) { }
    }

    PiwikTracker.prototype.OnPageEditButton = function () {
        var saveEditInterval = setInterval(function () {
            var pageSaveButton = document.getElementById('Ribbon.EditingTools.CPEditTab.EditAndCheckout.SaveEdit-SelectedItem');
            if (pageSaveButton != undefined && pageSaveButton != null) {
                var saveEditfunc = pageSaveButton.getAttribute('onclick');
                if (saveEditfunc.indexOf('PiwikTrackerInstance.OnPageEditing();') < 0) {
                    pageSaveButton.setAttribute('onclick', 'PiwikTrackerInstance.OnPageEditing();' + saveEditfunc + ';');
                    clearInterval(saveEditInterval);
                }
            }
        }, 1000);
    }

    PiwikTracker.prototype.OverrideModernPublishButton = function () {
        var self = this;
        PiwikTrackerInstance.OnModernPublishing = function () {
            window.setTimeout(function () {
                self.OverrideSiteSaveAndCloseSPO();
                self.OverrideModernSiteEditSPO();
            }, 3000);
        }

        var intervalId = setInterval(function () {
            var button = document.querySelectorAll("button[data-automation-id='pageCommandBarPublishButton']")[0];
            if (button != undefined && button != null) {
                clearInterval(intervalId);

                var editFunc = button.getAttribute('onclick');
                if (editFunc == null) {
                    editFunc = "";
                }
                if (editFunc.indexOf('PiwikTrackerInstance.OnModernPublishing();') < 0) {
                    button.setAttribute('onclick', 'PiwikTrackerInstance.OnModernPublishing();' + editFunc + ';');
                }
            }
        }, 1000);
    }

    PiwikTracker.prototype.OnPageEditing = function (overridePublishingButton) {
        this.Track(TrackerAction.PageEdited);

        if (overridePublishingButton) {
            this.OverrideModernPublishButton();
        }
    }

    PiwikTracker.prototype.ClearSessionKeyForPageAdded = function () {
        // Added to ensure that edit page goal will be tracked after click on Edit button
        if (sessionStorage[PiwikTrackerInstance.SESSION_PAGE_ADDED_KEY] === "true") {
            sessionStorage.removeItem(PiwikTrackerInstance.SESSION_PAGE_ADDED_KEY);
        }
    }

    PiwikTracker.prototype.OverrideDownload = function () {
        this.HookIntoModernDownloadButtons();

        var tempNavigate = window.Nav ? window.Nav.navigate : window.STSNavigate;

        var navigateOverrideFunction = function (url) {
            try {
                if (url && url.toLowerCase().indexOf('download.aspx') >= 0) {
                    var indexOfSource = url.toLowerCase().indexOf('sourceurl=');
                    if (indexOfSource >= 0) {
                        var downloadUrl = window.location.protocol + '//' + window.location.host + decodeURIComponent(url.substring(indexOfSource + 'sourceurl='.length).split('&')[0]);
                        _paq.push(['trackLink', downloadUrl, 'download']);
                    }
                }
            }
            finally {
                tempNavigate.apply(this, arguments);
            }
        }

        if (window.Nav) {
            window.Nav.navigate = navigateOverrideFunction;
        } else {
            window.STSNavigate = navigateOverrideFunction;
        }
    }

    PiwikTracker.prototype.getElementByIdContains = function (seachdoc, pattern, tagname) {
        var elems = seachdoc.getElementsByTagName(tagname);
        for (var i = 0; i < elems.length; i++) {
            if (elems[i].id.indexOf(pattern) >= 0) {
                return elems[i];
            }
        }
    }

    PiwikTracker.prototype.Track = function (action) {
        var customVarCounter = 0

        var currentdate = new Date();
        currentdate.setHours(0, 0, 0, 0);

        // Supported tracked actions from UI 
        var documentUpload = false;
        var pageAdded = false;
        var pageEdited = false;
        var searchMade = false;
        var searchword = '';

        if (action != null) {
            switch (action) {
                // Search made
                case TrackerAction.SiteSearch:

                    var searchbox = GetSearchBox();
                    try {
                        searchword = searchbox.value;
                        if (searchword.length > 0)
                            searchMade = true;
                    }
                    catch (err) { }
                    break;

                case TrackerAction.DocumentUpload:
                    documentUpload = true;
                    break;

                case TrackerAction.PageAdded:
                    {
                        sessionStorage[this.SESSION_PAGE_ADDED_KEY] = true;
                        pageAdded = true;
                    }
                    break;

                case TrackerAction.PageEdited:
                    {
                        if (sessionStorage[this.SESSION_PAGE_ADDED_KEY] === "true") {
                            sessionStorage.removeItem(this.SESSION_PAGE_ADDED_KEY);
                        }
                        else {
                            pageEdited = true;
                        }
                    }
                    break;
                case TrackerAction.DocumentLibrarySearch:
                    {
                        searchword = PiwikTrackerInstance.SearchContextSettings.searchTerm;
                        searchMade = true;
                        PiwikTrackerInstance.SearchContextSettings.searchTerm = null;
                    }
                    break;
            }
        }

        function GetSearchBox() {
            //classic sites searchbox
            var searchbox = PiwikTrackerInstance.getElementByIdContains(document, 'csr_sbox', 'input');
            if (searchbox == null) {
                //modern sites searchbox
                searchbox = document.querySelector("input[type='search']") || document.querySelector("input[role='search']");
            }

            return searchbox;
        }

        // Detect search
        var isModernQuickSearchEntered = location.href.match("view=7&q=") != null;
        if (document.URL.toLowerCase().indexOf(this.PIWIK_SEARCH_PAGE) > 0 || document.URL.toLowerCase().indexOf(this.PIWIK_SEARCH_PAGE_MODERN) > 0 || isModernQuickSearchEntered) {
            try {
                searchword = GetSearchBox().value;
                if (searchword.length > 0) {
                    searchMade = true;
                }
            }
            catch (err) {
            }
        }

        window._paq = window._paq || [];

        if (this.SiteTrackerSettings.sendUserExtendedInfo == "true") {
            // Send user name
            if (this.SiteTrackerSettings.sendUsername == "true") {
                customVarCounter++;
                if (this.ContextSettings.currentUserName != '')
                    window._paq.push(['setCustomVariable', customVarCounter, 'User name', currentUserNameHashedOrNot, 'visit']);
            }

            // Send office
            if (this.SiteTrackerSettings.sendOffice == "true") {
                customVarCounter++;
                if (this.ContextSettings.office != '')
                    window._paq.push(['setCustomVariable', customVarCounter, 'Office', this.ContextSettings.office, 'visit']);
            }

            // Send job title
            if (this.SiteTrackerSettings.sendJobTitle == "true") {
                customVarCounter++;
                if (this.ContextSettings.jobTitle != '')
                    window._paq.push(['setCustomVariable', customVarCounter, 'Job title', this.ContextSettings.jobTitle, 'visit']);
            }

            // Send department
            if (this.SiteTrackerSettings.sendDepartment == "true") {
                customVarCounter++;
                if (this.ContextSettings.department != '')
                    window._paq.push(['setCustomVariable', customVarCounter, 'Department', this.ContextSettings.department, 'visit']);
            }
        }

        // Send user name
        if (currentUserLoginHashedOrNot)
            window._paq.push(['setUserId', currentUserLoginHashedOrNot]);

        // Send visited page title
        window._paq.push(['setDocumentTitle', this.getDocumentTitleForTracker()]);

        // Goal: document added
        if (this.SiteTrackerSettings.useGoalDocumentAdded == "true") {
            if (documentUpload)
                window._paq.push(['trackGoal', this.SiteTrackerSettings.goalDocumentAddedId]);
        }

        // Goal: Page added
        if (this.SiteTrackerSettings.useGoalPageAdded == "true") {
            if (pageAdded)
                window._paq.push(['trackGoal', this.SiteTrackerSettings.goalPageAddedId]);
        }

        // Goal: Page edited
        if (this.SiteTrackerSettings.useGoalPageEdited == "true") {
            if (pageEdited == true) {
                window._paq.push(['trackGoal', this.SiteTrackerSettings.goalPageEditedId]);
            }
        }

        // Send search made and search term
        if (searchMade === true && (action === TrackerAction.SiteSearch || action === TrackerAction.DocumentLibrarySearch)) {
            var categories = null;
            if (this.SearchContextSettings.searchCategories) {
                categories = this.SearchContextSettings.searchCategories.join(', ');
            }

            window._paq.push(["trackSiteSearch", searchword, categories, this.SearchContextSettings.searchResultsCount]);
        } else {
            window._paq.push(['setCustomUrl', location.href.toLowerCase()]);
        }

        if (!documentUpload && !searchMade) {
            window._paq.push(['trackPageView']);
        }

        window._paq.push(['setLinkTrackingTimer', 1500]);

        var self = this;
        var protocol = (this.SiteTrackerSettings.enforceSSL == "true") ? 'https' : (('https:' == document.location.protocol) ? 'https' : 'http');

        (function () {
            var u = protocol + '://' + PiwikTrackerInstance.SiteTrackerSettings.piwikUrl + '/';
            window._paq.push(['setTrackerUrl', u + 'ppms.php']);
            window._paq.push(['setSiteId', PiwikTrackerInstance.SiteTrackerSettings.piwikSiteUUId]);
            if (self._jsLoaded) {
                return;
            }
            var d = document, g = d.createElement('script'), s = d.getElementsByTagName('script')[0]; g.type = 'text/javascript';
            g.defer = true; g.async = true; g.src = u + 'piwik.js'; s.parentNode.insertBefore(g, s);
            self._jsLoaded = true;
        })();
    }

    PiwikTracker.prototype.ValidateInstallation = function () {

        var self = this;

        SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {

            var ctx = SP.ClientContext.get_current();
            var web = ctx.get_web();
            var appInstances = SP.AppCatalog.getAppInstances(ctx, web);
            ctx.load(appInstances);
            ctx.executeQueryAsync(
                function () {
                    var isInstalled = false;
                    var apps = appInstances.getEnumerator();

                    while (apps.moveNext()) {

                        var title = apps.get_current().get_title();
                        if (title == "Piwik PRO") {
                            isInstalled = true;
                            break;
                        }
                    }

                    // TODO: Needs to be refactored
                    //if (!isInstalled) {
                    //    self.showStatus('Piwik PRO app has been removed. Click <a href="#" onClick="PiwikTrackerInstance.RemoveScript()">here</a> to uninstall script.', 'yellow');
                    //}
                },
                function (sender, args) {
                    console.log(args.get_message());
                });
        });
    }

    PiwikTracker.prototype.RemoveScript = function () {

        var self = this;

        SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {

            var currentcontext = new SP.ClientContext.get_current();
            var userCustomActions = currentcontext.get_site().get_userCustomActions();

            currentcontext.load(userCustomActions);
            currentcontext.executeQueryAsync(function () {

                var actionsToDelete = userCustomActions.get_data().filter(function (customAction) {
                    if (customAction.get_title() === "PiwikPRO")
                        return customAction;
                });

                for (var i = 0; i < actionsToDelete.length; i++) {
                    actionsToDelete[i].deleteObject();
                }

                currentcontext.executeQueryAsync(
                    function () { self.showStatus("Done", 'yellow'); },
                    function () { self.showStatus("Something went wrong..."); });

            }, function () { self.showStatus("Something went wrong..."); });
        })
    };

    PiwikTracker.prototype.CheckLicense = function (callback) {

        var self = this;

        function storePiwikData(piwikData) {

            if (piwikData) {
                document.cookie = "PIWIK_SP_DATA=" + JSON.stringify(piwikData) + "; expires=" + new Date(piwikData.licenseExpirationDate).toUTCString() + "; path=/";
            }
        }

        function readPiwikData() {
            var c_name = "PIWIK_SP_DATA";
            var i, cookieName, y, cookies = document.cookie.split(";");
            for (i = 0; i < cookies.length; i++) {
                cookieName = cookies[i].substr(0, cookies[i].indexOf("=")).replace(/^\s+|\s+$/g, "");;
                cookieValue = cookies[i].substr(cookies[i].indexOf("=") + 1);

                if (cookieName == c_name) {
                    try {
                        return JSON.parse(cookieValue);
                    }
                    catch (e) {
                        return null;
                    }
                }
            }

            return null;
        }

        var piwikData = readPiwikData();
        if (piwikData && piwikData.licenseValid) {
            callback();
            return;
        }

        var appProxyIframe = document.createElement("iframe");
        var licenseHost = "@licenseHost";
        var licenseHostUrlParma = (licenseHost.length > 0 && licenseHost != "null") ? ("&caUrl=" + encodeURIComponent(licenseHost)) : "";

        appProxyIframe.setAttribute("src", "@appUrl" + "appProxy?SPHostUrl=" + encodeURIComponent(_spPageContextInfo.webAbsoluteUrl) + licenseHostUrlParma);

        appProxyIframe.style.display = "none";
        appProxyIframe.id = "PiwikAppProxy";
        // document.body.appendChild(appProxyIframe);

        window.addEventListener('message', function (e) {
            var key = e.message ? 'message' : 'data';
            var data = e[key];

            if (data && data.licenseValid != undefined) {

                storePiwikData(data);

                var piwikData = readPiwikData();
                if (piwikData && piwikData.licenseValid) {
                    callback();
                } else {
                    self.showStatus('Piwik license has expired!');
                }
            }


        }, false);
    }
}

//(function () {

//    var errorOccurred = false;
//    var piwikInitInvoked = false;
//    var showConsoleInfo = true;

//    try {
//        @customScript

//        // Run tracker
//        if (_spPageContextInfo == null) {
//            if (_legacySPPageContextInfo == null) {
//                console.log("No SharePoint Context Info object found!");
//            }

//            _spPageContextInfo = _legacySPPageContextInfo;
//        }

//        RunPiwikTracker();
//        EmbedTagManagerScript();
//        piwikInitInvoked = true;
//    }
//    catch (e) {
//        errorOccurred = true;
//        throw e;
//    }
//    finally {
//        if (showConsoleInfo && !piwikInitInvoked) {
//            if (errorOccurred) {
//                console.log('Piwik PRO has not been initialized on load due to error.');
//            }
//            else {
//                console.log('Piwik PRO has not been initialized on load. Please run "RunPiwikTracker" manually or set "showConsoleInfo" variable to false in custom script section.');
//            }
//        }
//    }

//})();

function ExecuteFunctionForPiwikTracking() {
    var errorOccurred = false;
    var piwikInitInvoked = false;
    var showConsoleInfo = true;

    try {
        //@customScript

        if (_spPageContextInfo == null) {
            if (_legacySPPageContextInfo == null) {
                console.log("No SharePoint Context Info object found!");
            }

            _spPageContextInfo = _legacySPPageContextInfo;
        }

        // Run tracker
        RunPiwikTracker();
        EmbedTagManagerScript();
        piwikInitInvoked = true;
    }
    catch (e) {
        errorOccurred = true;
        throw e;
    }
    finally {
        if (showConsoleInfo && !piwikInitInvoked) {
            if (errorOccurred) {
                console.log('Piwik PRO has not been initialized on load due to error.');
            }
            else {
                console.log('Piwik PRO has not been initialized on load. Please run "RunPiwikTracker" manually or set "showConsoleInfo" variable to false in custom script section.');
            }
        }
    }
}

var oPropBag;
var varoWeb;
this.PropertyBagSettingsSet = {
    piwik_adminsiteurl: "",
    piwik_containersurl: "",
    piwik_enforcessl: "",
    piwik_trackerjsscripturl: "",
    piwik_serviceurl: "",
    piwik_sha3: "",
    piwik_templatesenduserextendedinfo: "",
    piwik_templatesenduserencoded: "",
    piwik_templateusegoaldocumentadded: "",
    piwik_templateusegoalpageadded: "",
    piwik_templateusegoalpageedited: "",
    piwik_templatesendusername: "",
    piwik_templatesenddepartment: "",
    piwik_templatesendjobtitle: "",
    piwik_templatesendoffice: "",
    piwik_senddepartment: "",
    piwik_goaldocumentaddedid: "",
    isconnectedtometasite: "",
    piwik_sendjobtitle: "",
    piwik_metasitename: "",
    piwik_sendoffice: "",
    piwik_goalpageaddedid: "",
    piwik_goalpageeditedid: "",
    piwik_senduserextendedinfo: "",
    piwik_senduserencoded: "",
    piwik_usegoaldocumentadded: "",
    piwik_usegoalpageadded: "",
    piwik_usegoalpageedited: "",
    piwik_metasitenamestored: "",
    piwik_istrackingactive: "",
    piwik_sendusername: "",
    piwik_clientid: "",
    piwik_clientsecret: "",
    piwik_oldapitoken: "",

    Department: "piwik_senddepartment",
    DocumentAddedGoalId: "piwik_goaldocumentaddedid",
    EnforceSslComunication: "piwik_enforcessl",
    IsConnectedToMetaSite: "isconnectedtometasite",
    JobTitle: "piwik_sendjobtitle",
    MetaSiteId: "piwik_metasitename",
    Office: "piwik_sendoffice",
    PageAddedGoalId: "piwik_goalpageaddedid",
    PageEditedGoalId: "piwik_goalpageeditedid",
    SendExtendedUserinfo: "piwik_senduserextendedinfo",
    SendUserIdEncoded: "piwik_senduserencoded",
    ShouldTrackDocumentAddedGoal: "piwik_usegoaldocumentadded",
    ShouldTrackPageAddedGoal: "piwik_usegoalpageadded",
    ShouldTrackPageEditedGoal: "piwik_usegoalpageedited",
    SiteId: "piwik_metasitenamestored",
    Url: "piwik_serviceurl",
    UserName: "piwik_sendusername",
    ContainersUrl: "piwik_containersurl",
    IsTrackingActive: "piwik_istrackingactive",
    TrackerJSScriptURL: "piwik_trackerjsscripturl",
    ClientID: "piwik_clientid",
    ClientSecret: "piwik_clientsecret",
    OldAPIToken: "piwik_oldapitoken",
    TemplateSendExtendedUserinfo: "piwik_templatesenduserextendedinfo",
    TemplateSendUserIdEncoded: "piwik_templatesenduserencoded",
    TemplateShouldTrackDocumentAddedGoal: "piwik_templateusegoaldocumentadded",
    TemplateShouldTrackPageAddedGoal: "piwik_templateusegoalpageadded",
    TemplateShouldTrackPageEditedGoal: "piwik_templateusegoalpageedited",
    TemplateUserName: "piwik_templatesendusername",
    TemplateDepartment: "piwik_templatesenddepartment",
    TemplateJobTitle: "piwik_templatesendjobtitle",
    TemplateOffice: "piwik_templatesendoffice",
    UseSha3: "piwik_sha3"
}


var oPropBagPiwikAdmin;// = new Map();
var varoWebPiwikAdmin;//= new Map();
var isRunning = false;
var piwikNotFullyActivated = false;

function ReadPropertyBagFromPiwikAdmin() {
    if (isRunning == false) {
        isRunning = true;
        //Get Client Context and Web object.  
        var piwikAdminUrl = CallFarmServiceForProperty("piwik_adminsiteurl");
        if (piwikAdminUrl.substr(piwikAdminUrl.length - 1) != "/") {
            piwikAdminUrl = piwikAdminUrl + "/";
        }
        PropertyBagSettingsSet.piwik_adminsiteurl = piwikAdminUrl;
        var clientContextPiwikAdmin = new SP.ClientContext(piwikAdminUrl);

        varoWebPiwikAdmin = clientContextPiwikAdmin.get_web();

        //Get property bag collection
        oPropBagPiwikAdmin = varoWebPiwikAdmin.get_allProperties();

        //Load client context and execute the batch
        clientContextPiwikAdmin.load(oPropBagPiwikAdmin);
        clientContextPiwikAdmin.executeQueryAsync(SuccessPiwikAdmin, FailurePiwikAdmin);
    }
}
function SuccessPiwikAdmin() {
    var propBagsPiwikAdmin = oPropBagPiwikAdmin.get_fieldValues();
    PropertyBagSettingsSet.piwik_containersurl = propBagsPiwikAdmin["piwik_containersurl"];
    PropertyBagSettingsSet.piwik_enforcessl = propBagsPiwikAdmin["piwik_enforcessl"];
    PropertyBagSettingsSet.piwik_trackerjsscripturl = propBagsPiwikAdmin["piwik_trackerjsscripturl"];
    PropertyBagSettingsSet.piwik_serviceurl = propBagsPiwikAdmin["piwik_serviceurl"];
    PropertyBagSettingsSet.piwik_sha3 = propBagsPiwikAdmin["piwik_sha3"];
    PropertyBagSettingsSet.piwik_templatesenduserextendedinfo = propBagsPiwikAdmin["piwik_templatesenduserextendedinfo"];
    PropertyBagSettingsSet.piwik_templatesenduserencoded = propBagsPiwikAdmin["piwik_templatesenduserencoded"];
    PropertyBagSettingsSet.piwik_templateusegoaldocumentadded = propBagsPiwikAdmin["piwik_templateusegoaldocumentadded"];
    PropertyBagSettingsSet.piwik_templateusegoalpageadded = propBagsPiwikAdmin["piwik_templateusegoalpageadded"];
    PropertyBagSettingsSet.piwik_templateusegoalpageedited = propBagsPiwikAdmin["piwik_templateusegoalpageedited"];
    PropertyBagSettingsSet.piwik_templatesendusername = propBagsPiwikAdmin["piwik_templatesendusername"];
    PropertyBagSettingsSet.piwik_templatesenddepartment = propBagsPiwikAdmin["piwik_templatesenddepartment"];
    PropertyBagSettingsSet.piwik_templatesendjobtitle = propBagsPiwikAdmin["piwik_templatesendjobtitle"];
    PropertyBagSettingsSet.piwik_templatesendoffice = propBagsPiwikAdmin["piwik_templatesendoffice"];
    ReadPropertyBag();
}
function FailurePiwikAdmin(sender, args) {
    console.log('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}

function CallFarmServiceForProperty(property) {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", window.location.protocol + '//' + window.location.hostname + '/_vti_bin/CUSTOM/PiwikPROFarmOperationsService.svc/GetFarmProperty?propName=' + property + '&propTemp_=' + new Date().getTime(), false);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send(null);
    return xhttp.responseText.replace(/\\/g, "").replace(/\"/g, "");
}


function ReadPropertyBag() {
    //Get Client Context and Web object.  
    clientContext = new SP.ClientContext.get_current();
    varoWeb = clientContext.get_web();

    //Get property bag collection
    oPropBag = varoWeb.get_allProperties();

    //Load client context and execute the batch
    clientContext.load(oPropBag);
    clientContext.executeQueryAsync(Success, Failure);
}

function Success() {
    var propBags = oPropBag.get_fieldValues();
    if (propBags["piwik_senddepartment"] === undefined) {
        piwikNotFullyActivated = true;
    }
    PropertyBagSettingsSet.piwik_senddepartment = propBags["piwik_senddepartment"];
    PropertyBagSettingsSet.piwik_goaldocumentaddedid = propBags["piwik_goaldocumentaddedid"];
    PropertyBagSettingsSet.isconnectedtometasite = propBags["isconnectedtometasite"];
    PropertyBagSettingsSet.piwik_sendjobtitle = propBags["piwik_sendjobtitle"];
    PropertyBagSettingsSet.piwik_metasitename = propBags["piwik_metasitename"];
    PropertyBagSettingsSet.piwik_sendoffice = propBags["piwik_sendoffice"];
    PropertyBagSettingsSet.piwik_goalpageaddedid = propBags["piwik_goalpageaddedid"];
    PropertyBagSettingsSet.piwik_goalpageeditedid = propBags["piwik_goalpageeditedid"];
    PropertyBagSettingsSet.piwik_senduserextendedinfo = propBags["piwik_senduserextendedinfo"];
    PropertyBagSettingsSet.piwik_senduserencoded = propBags["piwik_senduserencoded"];
    PropertyBagSettingsSet.piwik_usegoaldocumentadded = propBags["piwik_usegoaldocumentadded"];
    PropertyBagSettingsSet.piwik_usegoalpageadded = propBags["piwik_usegoalpageadded"];
    PropertyBagSettingsSet.piwik_usegoalpageedited = propBags["piwik_usegoalpageedited"];
    PropertyBagSettingsSet.piwik_metasitenamestored = propBags["piwik_metasitenamestored"];
    PropertyBagSettingsSet.piwik_istrackingactive = propBags["piwik_istrackingactive"];
    PropertyBagSettingsSet.piwik_sendusername = propBags["piwik_sendusername"];
    PropertyBagSettingsSet.piwik_clientid = CallFarmServiceForProperty("piwik_clientid");
    PropertyBagSettingsSet.piwik_clientsecret = CallFarmServiceForProperty("piwik_clientsecret");
    PropertyBagSettingsSet.piwik_oldapitoken = CallFarmServiceForProperty("piwik_oldapitoken");

    //console.log("Getting values from property bag.. Success");
    PropertyBagsLoaded = true;

    ExecuteFunctionForPiwikTracking();
}

function Failure(sender, args) {
    console.log('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}


(function (funcName, baseObj) {
    // The public function name defaults to window.docReady
    // but you can pass in your own object and own function name and those will be used
    // if you want to put them in a different namespace
    funcName = funcName || "docReady";
    baseObj = baseObj || window;
    var readyList = [];
    var readyFired = false;
    var readyEventHandlersInstalled = false;

    // call this when the document is ready
    // this function protects itself against being called more than once
    function ready() {
        if (!readyFired) {
            // this must be set to true before we start calling callbacks
            readyFired = true;
            for (var i = 0; i < readyList.length; i++) {
                // if a callback here happens to add new ready handlers,
                // the docReady() function will see that it already fired
                // and will schedule the callback to run right after
                // this event loop finishes so all handlers will still execute
                // in order and no new ones will be added to the readyList
                // while we are processing the list
                readyList[i].fn.call(window, readyList[i].ctx);
            }
            // allow any closures held by these functions to free
            readyList = [];
        }
    }

    function readyStateChange() {
        if (document.readyState === "complete") {
            ready();
        }
    }

    // This is the one public interface
    // docReady(fn, context);
    // the context argument is optional - if present, it will be passed
    // as an argument to the callback
    baseObj[funcName] = function (callback, context) {
        if (typeof callback !== "function") {
            throw new TypeError("callback for docReady(fn) must be a function");
        }
        // if ready has already fired, then just schedule the callback
        // to fire asynchronously, but right away
        if (readyFired) {
            setTimeout(function () { callback(context); }, 1);
            return;
        } else {
            // add the function and context to the list
            readyList.push({ fn: callback, ctx: context });
        }
        // if document already ready to go, schedule the ready function to run
        if (document.readyState === "complete") {
            setTimeout(ready, 1);
        } else if (!readyEventHandlersInstalled) {
            // otherwise if we don't have event handlers installed, install them
            if (document.addEventListener) {
                // first choice is DOMContentLoaded event
                document.addEventListener("DOMContentLoaded", ready, false);
                // backup is window load event
                window.addEventListener("load", ready, false);
            } else {
                // must be IE
                document.attachEvent("onreadystatechange", readyStateChange);
                window.attachEvent("onload", ready);
            }
            readyEventHandlersInstalled = true;
        }
    }
})("docReady", window);


window.addEventListener("hashchange", funcIFLoaded, false);
var executionHashChange = false;
function funcIFLoaded() {
    isRunning = false;
    executionHashChange = true;
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
        SP.SOD.executeOrDelayUntilScriptLoaded(ReadPropertyBagFromPiwikAdmin, 'sp.js');
    });

    setTimeout(function () {
        SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
            SP.SOD.executeOrDelayUntilScriptLoaded(ReadPropertyBagFromPiwikAdmin, 'sp.js');
        });
    }, 2800);

}

docReady(function () {
    try {
        if (executionHashChange === false) {
            executionHashChange = true;
            SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
                SP.SOD.executeOrDelayUntilScriptLoaded(ReadPropertyBagFromPiwikAdmin, 'sp.js');
            });

            setTimeout(function () {
                SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
                    SP.SOD.executeOrDelayUntilScriptLoaded(ReadPropertyBagFromPiwikAdmin, 'sp.js');
                });
            }, 2800);
        }
    }
    catch (e) {
        throw e;
    }
});


var oneTimeLoading = false;

function GeneratePiwikForm() {
    if (oneTimeLoading == false) {
        oneTimeLoading = true;

        var elementPrototype = typeof HTMLElement !== "undefined"
            ? HTMLElement.prototype : Element.prototype;
        elementPrototype;

        elementPrototype.appendFirst = function (childNode) {
            if (this.firstChild) this.insertBefore(childNode, this.firstChild);
            else this.appendChild(childNode);
        };
        var elemLoading = document.createElement('div');
        elemLoading.setAttribute("id", "PiwikBackgroundDivLoading");
        elemLoading.style.cssText = 'position:absolute;width:100%;height:100%;opacity:0.3;z-index:100;background:#000;color:#fff;-ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=30)"';
        document.body.appendFirst(elemLoading);

        var elem2Loading = document.createElement('div');
        elem2Loading.setAttribute("id", "PiwikBackgroundDivLoading2");
        elem2Loading.style.cssText = 'position:absolute;top:50%;left:50%;margin-top:-200px;margin-left:-300px;width:600px;height:400px;z-index:101;color:#FFF;font-size:72px;text-align:center;font-weight:bold';
        elem2Loading.innerHTML = "Loading...";
        document.body.appendFirst(elem2Loading);
    }

    if (PropertyBagsLoaded == true) {
        try {
            removeElement('PiwikBackgroundDivLoading');
            removeElement('PiwikBackgroundDivLoading2');
        }
        catch (e) { }
        if (piwikNotFullyActivated) {
            alert("Activation of tracking doesn't take place right after the installation. SharePoint connects to Piwik PRO on schedule to synchronize configuration. Next synchronization will take place around full next hour.");
        }
        else {
            GeneratePiwikFormIfPropBagsLoaded();
        }
    }
    else {
        setTimeout(GeneratePiwikForm, 1000);
    }
}

function ShowWarningNotTrackingSite() {
    var elemWarningNotTracking = document.createElement('div');
    elemWarningNotTracking.setAttribute("id", "PiwikDivTrackingWarning");
    elemWarningNotTracking.style.cssText = 'background:yellow;color:#000';
    elemWarningNotTracking.innerHTML = "Warning: site has been removed from Piwik PRO and cannot be tracked!";
    document.body.appendFirst(elemWarningNotTracking);
}

function EventListenerForClickOnPiwikPRO() {
    removeElement('PiwikBackgroundDiv');
    removeElement('PiwikFormDiv');
    oneTimeLoading = false;
}

function GeneratePiwikFormIfPropBagsLoaded() {


    var elem = document.createElement('div');
    elem.setAttribute("id", "PiwikBackgroundDiv");
    elem.style.cssText = 'position:absolute;width:100%;height:100%;opacity:0.3;z-index:100;background:#000;-ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=30)"';
    document.body.appendFirst(elem);

    var elem2 = document.createElement('div');
    //elem2.innerHTML = //"<div id='Div_PiwikCloseForm' style='text-align:right;font-size:16px;cursor:pointer;float:right'>X</div>"+
    //				"<div style='font-size:18px'><p></p><p>Piwik Settings</p></div>";
    elem2.setAttribute("id", "PiwikFormDiv");
    elem2.style.cssText = 'position:absolute;top:50%;left:50%;margin-top:-400px;margin-left:-300px;width:600px;height:700px;z-index:101;background:#FFF;padding-left:20px;padding-top:4px;padding-right:12px';
    document.body.appendFirst(elem2);

    var elemPiwikSettings = document.createElement('div');
    elemPiwikSettings.innerHTML = "<div style='display:inline-block'><img height='35px' src='" + PropertyBagSettingsSet.piwik_adminsiteurl + PiwikImageLogoSrc + "'></img></div><div style='width:115px;font-size:20px;text-align:right;display:inline-block;vertical-align:super'><p></p><p><h2>Settings</h2></p><p></p></div>";
    elem2.appendFirst(elemPiwikSettings);

    var elemX = document.createElement('div');
    elemX.innerHTML = "x";
    elemX.setAttribute("id", "Div_PiwikCloseForm");
    elemX.setAttribute("style", "text-align:right;font-size:20px;cursor:pointer;float:right");
    elem2.appendFirst(elemX);

    if (elemX.addEventListener) {
        elemX.addEventListener("click", EventListenerForClickOnPiwikPRO, false);
    }
    else {
        elemX.attachEvent("onclick", EventListenerForClickOnPiwikPRO);
    }

    function CheckIfFarmAdmin() {
        var xhttp = new XMLHttpRequest();
        xhttp.open("GET", window.location.protocol + '//' + window.location.hostname + '/_vti_bin/CUSTOM/PiwikPROFarmOperationsService.svc/IsUserFarmAdmin', false);
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.send(null);
        return xhttp.responseText;
    }
    var isFarmAdmin = CheckIfFarmAdmin();
    //var isFarmAdmin = "true";

    var elemTopMenu = document.createElement('div');
    if (isFarmAdmin == "true") {
        elemTopMenu.innerHTML = "<div id='Div_PiwikMenu_SiteCollection' style='display:inline-block;font-weight:bold;margin-right:15px'>Site Collection Settings</div>" +
            "<div id='Div_PiwikMenu_PiwikAdmin' style='display:inline-block;margin-right:15px'>Global Settings</div>" +
            "<div id='Div_PiwikMenu_Farm' style='display:inline-block'>Connection Settings</div><hr/>";
    }
    else {
        elemTopMenu.innerHTML = "<div id='Div_PiwikMenu_SiteCollection' style='display:inline-block;font-weight:bold;margin-right:15px'>SiteCollection</div><hr/>";
    }
    elemTopMenu.setAttribute("id", "Div_PiwikTopMenu");
    elemTopMenu.setAttribute("style", "font-size:14px;cursor:pointer;overflow:hidden;margin-bottom:25px");
    elem2.appendChild(elemTopMenu);

    function renderInputCheckBoxForProperty(property, isDisabled) {
        var inputElemTracking = document.createElement("INPUT");
        inputElemTracking.setAttribute("type", "checkbox");
        if (isDisabled == true) {
            inputElemTracking.setAttribute("disabled", isDisabled);
        }
        inputElemTracking.setAttribute("id", "input_" + property);
        if (this.PropertyBagSettingsSet[this.PropertyBagSettingsSet[property]] == "true") {
            inputElemTracking.setAttribute("checked", "checked");
        }
        return inputElemTracking;
    }

    function renderInputTextBoxForProperty(property, isDisabled, width, textalign) {
        var inputElemTracking = document.createElement("INPUT");
        inputElemTracking.setAttribute("type", "text");
        if (isDisabled == true) {
            inputElemTracking.setAttribute("disabled", isDisabled);
        }
        inputElemTracking.setAttribute("id", "input_" + property);
        inputElemTracking.setAttribute("style", "border-color:#DDD;text-align:" + textalign + ";width:" + width);
        inputElemTracking.setAttribute("value", this.PropertyBagSettingsSet[this.PropertyBagSettingsSet[property]]);
        return inputElemTracking;
    }

    //Render SiteCollection tab
    var elemContentFormSiteCollection = document.createElement('div');
    elemContentFormSiteCollection.innerHTML = "<div id='Div_ContentFormSiteCollection_General' style='font-size:15px'><h3 style='font-weight:bold'>General</h3></div><p></p>" +
        "<div id='Div_ContentFormSiteCollection_SiteId' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Piwik PRO App ID:</div>" +
        "<div id='Div_ContentFormSiteCollection_SiteIdValue' style='display:inline-block'>" + renderInputTextBoxForProperty("SiteId", true, "240px", "center").outerHTML + "</div><div></div>" +
        "<div id='Div_ContentFormSiteCollection_IsTrackingActive' style='display:none;margin-bottom:5px;width:40%;margin-left:20px'>Tracking enabled:</div>" +
        "<div id='Div_ContentFormSiteCollection_IsTrackingActiveValue' style='display:none'>" + renderInputCheckBoxForProperty("IsTrackingActive", true).outerHTML + "</div><p/><p/>" +
        "<div id='Div_ContentFormSiteCollection_General' style='font-size:15px'><h3 style='font-weight:bold'>Goals (optional)</h3></div><p></p>" +
        "<div id='Div_ContentFormSiteCollection_DocumentAddedGoal' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Document Added:</div>" +
        "<div id='Div_ContentFormSiteCollection_DocumentAddedGoalValue' style='display:inline-block;margin-right:5px'>" + renderInputCheckBoxForProperty("ShouldTrackDocumentAddedGoal", false).outerHTML + "</div>" +
        "<div id='Div_ContentFormSiteCollection_DocumentAddedGoalIDValue' style='display:inline-block'>" + renderInputTextBoxForProperty("DocumentAddedGoalId", true, "30px", "center").outerHTML + "</div><p/><p/>" +
        "<div id='Div_ContentFormSiteCollection_PageAddedGoal' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Page Added:</div>" +
        "<div id='Div_ContentFormSiteCollection_PageAddedGoalValue' style='display:inline-block;margin-right:5px'>" + renderInputCheckBoxForProperty("ShouldTrackPageAddedGoal", false).outerHTML + "</div>" +
        "<div id='Div_ContentFormSiteCollection_PageAddedGoalIDValue' style='display:inline-block'>" + renderInputTextBoxForProperty("PageAddedGoalId", true, "30px", "center").outerHTML + "</div><p/><p/>" +
        "<div id='Div_ContentFormSiteCollection_PageEditedGoal' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Page Edited:</div>" +
        "<div id='Div_ContentFormSiteCollection_PageEditedGoalValue' style='display:inline-block;margin-right:5px'>" + renderInputCheckBoxForProperty("ShouldTrackPageEditedGoal", false).outerHTML + "</div>" +
        "<div id='Div_ContentFormSiteCollection_PageEditedGoalIDValue' style='display:inline-block'>" + renderInputTextBoxForProperty("PageEditedGoalId", true, "30px", "center").outerHTML + "</div><p/><p/>" +
        "<div id='Div_ContentFormSiteCollection_UserInformation' style='font-size:15px'><h3 style='font-weight:bold'>User Information</h3></div><p></p>" +
        "<div id='Div_ContentFormSiteCollection_SendUserIDEncoded' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Send User ID anonymized:</div>" +
        "<div id='Div_ContentFormSiteCollection_SendUserIDEncodedValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("SendUserIdEncoded", false).outerHTML + "</div><p/><p/>" +
        "<div id='Div_ContentFormSiteCollection_SendUserExtended' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Send extended user information:</div>" +
        "<div id='Div_ContentFormSiteCollection_SendUserExtendedValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("SendExtendedUserinfo", false).outerHTML + "</div><p/><p/>" +
        "<div id='Div_ContentFormSiteCollection_SendUsername' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:40px'>Username (display name):</div>" +
        "<div id='Div_ContentFormSiteCollection_SendUsernameValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("UserName", false).outerHTML + "</div><p/><p/>" +
        "<div id='Div_ContentFormSiteCollection_Office' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:40px'>Office:</div>" +
        "<div id='Div_ContentFormSiteCollection_OfficeValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("Office", false).outerHTML + "</div><p/><p/>" +
        "<div id='Div_ContentFormSiteCollection_JobTitle' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:40px'>JobTitle:</div>" +
        "<div id='Div_ContentFormSiteCollection_JobTitleValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("JobTitle", false).outerHTML + "</div><p/><p/>" +
        "<div id='Div_ContentFormSiteCollection_Department' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:40px'>Department:</div>" +
        "<div id='Div_ContentFormSiteCollection_DepartmentValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("Department", false).outerHTML + "</div><p/><p/><p></p>";
    elemContentFormSiteCollection.setAttribute("id", "Div_ContentFormSiteCollection");
    elemContentFormSiteCollection.setAttribute("style", "height:470px");
    elem2.appendChild(elemContentFormSiteCollection);

    if (isFarmAdmin == "true") {
        var elemContentFormPiwikAdmin = document.createElement('div');
        elemContentFormPiwikAdmin.innerHTML = "<div id='Div_ContentFormPiwikAdmin_Template' style='font-size:15px;font-weight:bold'><h3 style='font-weight:bold'>Default site collection settings</h3></div><p></p>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateDocumentAddedGoal' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Document Added:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateDocumentAddedGoalValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("TemplateShouldTrackDocumentAddedGoal", false).outerHTML + "</div><div></div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplatePageAddedGoal' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Page Added:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplatePageAddedGoalValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("TemplateShouldTrackPageAddedGoal", false).outerHTML + "</div><div></div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplatePageEditedGoal' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Page Edited:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplatePageEditedGoalValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("TemplateShouldTrackPageEditedGoal", false).outerHTML + "</div><div></div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateSendUserIDEncoded' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Send UserID Encoded:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateSendUserIDEncodedValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("TemplateSendUserIdEncoded", false).outerHTML + "</div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateSendUserExtended' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:20px'>Send extended user information:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateSendUserExtendedValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("TemplateSendExtendedUserinfo", false).outerHTML + "</div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateSendUsername' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:40px'>Username (display name):</div>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateSendUsernameValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("TemplateUserName", false).outerHTML + "</div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateOffice' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:40px'>Office:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateOfficeValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("TemplateOffice", false).outerHTML + "</div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateJobTitle' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:40px'>JobTitle:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateJobTitleValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("TemplateJobTitle", false).outerHTML + "</div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateDepartment' style='display:inline-block;margin-bottom:5px;width:40%;margin-left:40px'>Department:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_TemplateDepartmentValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("TemplateDepartment", false).outerHTML + "</div><div></div>";
        elemContentFormPiwikAdmin.setAttribute("id", "Div_ContentFormPiwikAdmin");
        elemContentFormPiwikAdmin.setAttribute("style", "display:none;height:470px");
        elem2.appendChild(elemContentFormPiwikAdmin);

        var elemContentFormFarm = document.createElement('div');
        elemContentFormFarm.innerHTML = "<div id='Div_ContentFormPiwikAdmin_GlobalSettings' style='font-size:15px'><h3 style='font-weight:bold'>Piwik PRO Analytics Suite instance connection settings</h3></div><p></p>" +
            "<div id='Div_ContentFormPiwikAdmin_EnforceSSLCommunication' style='display:inline-block;margin-bottom:5px;width:28%;margin-left:10px'>Enforce SSL Communication:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_EnforceSSLCommunicationValue' style='display:inline-block'>" + renderInputCheckBoxForProperty("EnforceSSLCommunication", false).outerHTML + "</div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_ServiceUrl' style='display:inline-block;margin-bottom:5px;width:28%;margin-left:10px'>Piwik PRO instance URL:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_ServiceUrlValue' style='display:inline-block'>" + renderInputTextBoxForProperty("Url", false, "400px", "left").outerHTML + "</div><div></div><p/><p/>" +
            "<div id='Div_ContentFormPiwikAdmin_ContainersUrl' style='display:inline-block;margin-bottom:5px;width:28%;margin-left:10px'>Custom Containers:</div>" +
            "<div id='Div_ContentFormPiwikAdmin_ContainersUrlValue' style='display:inline-block'>" + renderInputTextBoxForProperty("ContainersUrl", false, "400px", "left").outerHTML + "</div><div></div><p/><p/>" +
            //"<div id='Div_ContentFormPiwikAdmin_TrackerJSScriptURL' style='display:inline-block;margin-bottom:5px;width:28%;margin-left:10px'>JavaScript tracker path:</div>"+
            //"<div id='Div_ContentFormPiwikAdmin_TrackerJSScriptURLValue' style='display:inline-block'>"+renderInputTextBoxForProperty("TrackerJSScriptURL", false, //"400px","left").outerHTML+"</div><div></div><p/></p>"+
            "<div id='Div_ContentFormPiwikAdmin_UseSha3' style='display:inline-block;margin-bottom:5px;width:28%;margin-left:10px'>Use SHA3 to anonymize UserID (default SHA1):</div>" +
            "<div id='Div_ContentFormPiwikAdmin_UseSha3Value' style='display:inline-block'>" + renderInputCheckBoxForProperty("UseSha3", false).outerHTML + "<span class='tooltiptextSHA3'>If this option is checked the UserID is encrypted using the SHA3 algorithm. In other case system use SHA1 to encrypt the UserID.</span></div><p/><p/>" +
            "<div id='Div_ContentFormFarm_ClientID' style='display:inline-block;margin-bottom:5px;width:28%;margin-left:10px'>Piwik PRO Client ID:</div>" +
            "<div id='Div_ContentFormFarm_ClientIDValue' style='display:inline-block'>" + renderInputTextBoxForProperty("ClientID", false, "400px", "left").outerHTML + "</div><div></div><p/><p/>" +
            "<div id='Div_ContentFormFarm_ClientSecret' style='display:inline-block;margin-bottom:5px;width:28%;margin-left:10px'>Piwik PRO Client Secret:</div>" +
            "<div id='Div_ContentFormFarm_ClientSecretValue' style='display:inline-block'>" + renderInputTextBoxForProperty("ClientSecret", false, "400px", "left").outerHTML + "</div><div></div><p/><p/>" +
            "<div id='Div_ContentFormFarm_OldApiToken' style='display:inline-block;margin-bottom:5px;width:28%;margin-left:10px'>Piwik PRO Legacy authorization token:</div>" +
            "<div id='Div_ContentFormFarm_OldApiTokenValue' style='display:inline-block'>" + renderInputTextBoxForProperty("OldAPIToken", false, "400px", "left").outerHTML + "</div><div></div><p/><p/>" +
            "<div id='Div_ContentFormFarm_PiwikAdminSiteURL' style='display:inline-block;margin-bottom:5px;width:28%;margin-left:10px'>Piwik PRO integration site collection:</div>" +
            "<div id='Div_ContentFormFarm_PiwikAdminSiteURLValue' style='display:inline-block;vertical-align:super'><a href='" + this.PropertyBagSettingsSet.piwik_adminsiteurl + "'>" + this.PropertyBagSettingsSet.piwik_adminsiteurl + "</a></div><div></div>";
        elemContentFormFarm.setAttribute("id", "Div_ContentFormFarm");
        elemContentFormFarm.setAttribute("style", "display:none;height:470px");
        elem2.appendChild(elemContentFormFarm);
    }

    function isIE(version, comparison) {
        var cc = 'IE',
            b = document.createElement('B'),
            docElem = document.documentElement,
            isIE;

        if (version) {
            cc += ' ' + version;
            if (comparison) { cc = comparison + ' ' + cc; }
        }

        b.innerHTML = '<!--[if ' + cc + ']><b id="iecctest"></b><![endif]-->';
        docElem.appendChild(b);
        isIE = !!document.getElementById('iecctest');
        docElem.removeChild(b);
        return isIE;
    }



    var stylespp = "#Div_ContentFormPiwikAdmin_UseSha3Value {" +
        "position: relative;" +
        "display: inline-block;" +
        "}" +
        "#Div_ContentFormPiwikAdmin_UseSha3Value .tooltiptextSHA3 {" +
        "visibility: hidden;" +
        "width: 280px;" +
        "background-color: #555;" +
        "color: #fff;" +
        "text-align: center;" +
        "border-radius: 6px;" +
        "padding: 5px 0;" +
        "position: absolute;" +
        "z-index: 1;" +
        "bottom: 125%;" +
        "left: 50%;" +
        "margin-left: -140px;" +
        "opacity: 0;" +
        "transition: opacity 0.3s;" +
        "}" +
        "#Div_ContentFormPiwikAdmin_UseSha3Value .tooltiptextSHA3::after {" +
        "content: '';" +
        "position: absolute;" +
        "top: 100%;" +
        "left: 50%;" +
        "margin-left: -5px;" +
        "border-width: 5px;" +
        "border-style: solid;" +
        "border-color: #555 transparent transparent transparent;" +
        "}" +
        "#Div_ContentFormPiwikAdmin_UseSha3Value:hover .tooltiptextSHA3 {" +
        "visibility: visible;" +
        "opacity: 1;" +
        "}";

    var styleSheet = document.createElement("style");

    if (isIE(8) == true) {
        document.getElementsByTagName("head")[0].appendChild(styleSheet);
        styleSheet.setAttribute('type', 'text/css');
        styleSheet.styleSheet.cssText = stylespp;
    }
    else {
        var styleSheet = document.createElement("style");
        styleSheet.type = "text/css";
        styleSheet.innerText = stylespp;
        document.head.appendChild(styleSheet);
    }


    var elemContentErrorMsg = document.createElement('div');
    elemContentErrorMsg.innerHTML = "<p></p>";
    elemContentErrorMsg.setAttribute("id", "Div_ErrorMessage");
    elemContentErrorMsg.setAttribute("style", "display:none;text-align:right;color:red;margin-right:50px");
    elem2.appendChild(elemContentErrorMsg);

    var elemContentButtonOK = document.createElement('div');
    elemContentButtonOK.innerHTML = "<input id='PiwikFormSave' type='button' value='OK' style='background-color:#0072c6;color:#fff'></input>";
    elemContentButtonOK.setAttribute("id", "Div_ContentPiwikButtonOK");
    elemContentButtonOK.setAttribute("style", "display:inline-block;margin-left:400px");
    elem2.appendChild(elemContentButtonOK);

    if (elemContentButtonOK.addEventListener) {
        elemContentButtonOK.addEventListener("click", ActionOnButtonClickOK, false);
    }
    else {
        elemContentButtonOK.attachEvent("onclick", ActionOnButtonClickOK);
    }

    function ActionOnButtonClickOK() {
        var errorMessage = ValidateFormItems();
        if (errorMessage) {
            document.getElementById("Div_ErrorMessage").style.display = "block";
            document.getElementById("Div_ErrorMessage").innerHTML = errorMessage + "<p></p>";
        }
        else {
            document.getElementById("Div_ErrorMessage").style.display = "none";
            PiwikCheckIfChangedSiteCollectionPropBagsAndUpdate();
            if (isFarmAdmin == "true") {
                PiwikCheckIfChangedPiwikAdminPropBagsAndUpdate();
                PiwikCheckIfChangedFarmPropBagsAndUpdate();
            }
            removeElement('PiwikBackgroundDiv');
            removeElement('PiwikFormDiv');
            oneTimeLoading = false;
        }
    }

    function ValidateFormItems() {
        //validate containersUrl
        if (!document.getElementById("input_ContainersUrl").value.startsWith("//")) {
            return 'Custom Containers field must starts with \"//\"';
        }

        return "";
    }

    var propsToModifyOnPiwikAdminList;

    function PiwikCheckIfChangedSiteCollectionPropBagsAndUpdate() {
        var returnerPropBags = "";
        returnerPropBags = PiwikCheckIfValueHasBeenChanged("ShouldTrackDocumentAddedGoal", true) + PiwikCheckIfValueHasBeenChanged("ShouldTrackPageAddedGoal", true) + PiwikCheckIfValueHasBeenChanged("ShouldTrackPageEditedGoal", true) + PiwikCheckIfValueHasBeenChanged("SendUserIdEncoded", true) + PiwikCheckIfValueHasBeenChanged("SendExtendedUserinfo", true) + PiwikCheckIfValueHasBeenChanged("UserName", true) + PiwikCheckIfValueHasBeenChanged("Office", true) + PiwikCheckIfValueHasBeenChanged("JobTitle", true) + PiwikCheckIfValueHasBeenChanged("Department", true);

        if (returnerPropBags !== "") {
            var propsToModify = returnerPropBags.split(";");

            clientContext = new SP.ClientContext.get_current();
            varoWebModifyPropBag = clientContext.get_web();
            oPropBagModify = varoWebModifyPropBag.get_allProperties();

            for (i = 0; i < propsToModify.length; i++) {
                if (propsToModify[i] !== "") {
                    oPropBagModify.set_item(this.PropertyBagSettingsSet[propsToModify[i].split("|")[0]], propsToModify[i].split("|")[1]);
                    this.PropertyBagSettingsSet[this.PropertyBagSettingsSet[propsToModify[i].split("|")[0]]] = propsToModify[i].split("|")[1];
                }
            }
            varoWebModifyPropBag.update();
            clientContext.executeQueryAsync(SuccessModifyPropBag, FailureModifyPropBag);
            propsToModifyOnPiwikAdminList = returnerPropBags;
            UpdateItemOnPiwikAdminList();
        }
        return returnerPropBags;
    }

    function PiwikCheckIfChangedPiwikAdminPropBagsAndUpdate() {
        var returnerPropBags;
        returnerPropBags = PiwikCheckIfValueHasBeenChanged("EnforceSSLCommunication", true) + PiwikCheckIfValueHasBeenChanged("Url", false) + PiwikCheckIfValueHasBeenChanged("ContainersUrl", false) + //PiwikCheckIfValueHasBeenChanged("TrackerJSScriptURL", false) +
            PiwikCheckIfValueHasBeenChanged("UseSha3", true) + PiwikCheckIfValueHasBeenChanged("TemplateShouldTrackDocumentAddedGoal", true) + PiwikCheckIfValueHasBeenChanged("TemplateShouldTrackPageAddedGoal", true) + PiwikCheckIfValueHasBeenChanged("TemplateShouldTrackPageEditedGoal", true) + PiwikCheckIfValueHasBeenChanged("TemplateSendUserIdEncoded", true) + PiwikCheckIfValueHasBeenChanged("TemplateSendExtendedUserinfo", true) + PiwikCheckIfValueHasBeenChanged("TemplateUserName", true) + PiwikCheckIfValueHasBeenChanged("TemplateOffice", true) + PiwikCheckIfValueHasBeenChanged("TemplateJobTitle", true) + PiwikCheckIfValueHasBeenChanged("TemplateDepartment", true);

        if (returnerPropBags !== "") {
            var propsToModify = returnerPropBags.split(";");

            clientContextPiwikAdmin = new SP.ClientContext(this.PropertyBagSettingsSet.piwik_adminsiteurl);
            varoWebModifyPropBag = clientContextPiwikAdmin.get_web();
            oPropBagModify = varoWebModifyPropBag.get_allProperties();

            for (i = 0; i < propsToModify.length; i++) {
                if (propsToModify[i] !== "") {
                    oPropBagModify.set_item(this.PropertyBagSettingsSet[propsToModify[i].split("|")[0]], propsToModify[i].split("|")[1]);
                    this.PropertyBagSettingsSet[this.PropertyBagSettingsSet[propsToModify[i].split("|")[0]]] = propsToModify[i].split("|")[1];
                }
            }
            varoWebModifyPropBag.update();
            clientContextPiwikAdmin.executeQueryAsync(SuccessModifyPropBagAdmin, FailureModifyPropBagAdmin);
        }
        return returnerPropBags;
    }

    function PiwikCheckIfChangedFarmPropBagsAndUpdate() {
        var returnerPropBags;
        returnerPropBags = PiwikCheckIfValueHasBeenChanged("ClientID", false) + PiwikCheckIfValueHasBeenChanged("ClientSecret", false) + PiwikCheckIfValueHasBeenChanged("OldAPIToken", false);
        //+ PiwikCheckIfValueHasBeenChanged("PiwikAdminSiteUrl", false);

        if (returnerPropBags !== "") {
            var propsToModify = returnerPropBags.split(";");

            for (i = 0; i < propsToModify.length; i++) {
                if (propsToModify[i] !== "") {
                    ModifyPropertyBagOnFarm(propsToModify[i].split("|")[0], propsToModify[i].split("|")[1]);
                }
            }

        }
        return returnerPropBags;
    }

    function ModifyPropertyBagOnFarm(propertyKey, propertyValue) {
        var xhttp = new XMLHttpRequest();
        xhttp.open("GET", window.location.protocol + '//' + window.location.hostname + '/_vti_bin/CUSTOM/PiwikPROFarmOperationsService.svc/UpdateFarmPropertyCentral?propName=' + this.PropertyBagSettingsSet[propertyKey] + '&propValue=' + propertyValue, false);
        xhttp.setRequestHeader("Content-type", "application/json");
        xhttp.send(null);
        if (xhttp.responseText.replace(/\\/g, "").replace(/\"/g, "") == "true") {
            this.PropertyBagSettingsSet[this.PropertyBagSettingsSet[propertyKey]] = propertyValue;
        }
        return xhttp.responseText;
    }


    function PiwikCheckIfValueHasBeenChanged(property, isCheckBox) {
        var propertyToReturn = "";
        if (isCheckBox === true) {
            var isTrueSet = (this.PropertyBagSettingsSet[this.PropertyBagSettingsSet[property]] === 'true');
            if (document.getElementById("input_" + property).checked !== isTrueSet) {
                propertyToReturn = property + "|" + document.getElementById("input_" + property).checked + ";";
            }
        }
        else {
            if (document.getElementById("input_" + property).value !== this.PropertyBagSettingsSet[this.PropertyBagSettingsSet[property]]) {
                propertyToReturn = property + "|" + document.getElementById("input_" + property).value + ";";
            }
        }
        return propertyToReturn;
    }

    var varoWebModifyPropBag;
    var oPropBagModify;

    function SuccessModifyPropBag() {
        //console.log('Property bag value modified');
    }
    function FailureModifyPropBag(sender, args) {
        console.log('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
    }

    function SuccessModifyPropBagAdmin() {
        //console.log('Property bag value modified');
    }
    function FailureModifyPropBagAdmin(sender, args) {
        console.log('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
    }

    var collListItem;
    var clientContextPiwikAdmin2;
    function UpdateItemOnPiwikAdminList() {
        clientContextPiwikAdmin2 = new SP.ClientContext(this.PropertyBagSettingsSet.piwik_adminsiteurl);
        var mList = clientContextPiwikAdmin2.get_web().get_lists().getByTitle('Piwik Pro Site Directory');

        var camlQuery = new SP.CamlQuery();
        camlQuery.set_viewXml('<View>' +
            '<Query>' +
            '<Where><Eq><FieldRef Name=\'pwk_siteId\' /><Value Type=\'Text\'>' + this.PropertyBagSettingsSet.piwik_metasitenamestored + '</Value></Eq></Where>' +
            '</Query>' +
            '<ViewFields><FieldRef Name=\'pwk_auditlog\' /><FieldRef Name=\'pwk_siteId\' /><FieldRef Name=\'pwk_status\' /></ViewFields></View>');

        collListItem = mList.getItems(camlQuery);
        clientContextPiwikAdmin2.load(collListItem);
        clientContextPiwikAdmin2.executeQueryAsync(Function.createDelegate(this, onQuerySucceeded1), Function.createDelegate(this, onQueryFailed1));
    }
    var website;
    var currentUserLoaded;
    function onQuerySucceeded1() {
        var context = new SP.ClientContext.get_current();
        website = context.get_web();
        currentUserLoaded = website.get_currentUser();
        context.load(currentUserLoaded);
        context.executeQueryAsync(Function.createDelegate(this, onQuerySucceededLoadUser), Function.createDelegate(this, onQueryFailedLoadUser));

    }

    function onQuerySucceededLoadUser() {
        var currentUserLogin = currentUserLoaded.get_loginName();
        var currentUserTitle = currentUserLoaded.get_title();
        var listItemEnumerator = collListItem.getEnumerator();
        while (listItemEnumerator.moveNext()) {

            var curritem = listItemEnumerator.get_current();
            var oldauditlog = curritem.get_item("pwk_auditlog");
            if (oldauditlog == null) oldauditlog = "";
            var now = new Date();
            curritem.set_item("pwk_auditlog", oldauditlog + now.format("dd/MM/yyyy hh:mm") + ", User: " + currentUserTitle + " (" + currentUserLogin + "), Changed : " + propsToModifyOnPiwikAdminList + "\n");
            curritem.update();

            clientContextPiwikAdmin2.executeQueryAsync(Function.createDelegate(this, onQuerySucceeded), Function.createDelegate(this, onQueryFailed));
        }
    }
    function onQueryFailedLoadUser(sender, args) {
        alert('request failed ' + args.get_message() + '\n' + args.get_stackTrace());
    }

    function onQueryFailed1(sender, args) {
        console.log("failed to update list item on site directory list");
    }
    function onQuerySucceeded() {
        //console.log("list item updated");
    }
    function onQueryFailed(sender, args) {
        console.log("failed to update list item on site directory list: " + args.get_message() + '\n' + args.get_stackTrace());
    }

    var elemContentButtonCancel = document.createElement('div');
    elemContentButtonCancel.innerHTML = "<input id='PiwikFormCancel' type='button' value='Cancel'></input>";
    elemContentButtonCancel.setAttribute("id", "Div_ContentPiwikButtonCancel");
    elemContentButtonCancel.setAttribute("style", "display:inline-block");
    elem2.appendChild(elemContentButtonCancel);

    var elemContentTextDeactivate = document.createElement('div');
    elemContentTextDeactivate.innerHTML = "<p></p>In order to deactivate tracking please go to <a href=" + window.location.protocol + "//" + window.location.host + _spPageContextInfo.siteServerRelativeUrl + "/_layouts/15/ManageFeatures.aspx?Scope=Site>Site Collection Features</a> and deactivate the PiwikPRO.SiteCollectionTracking feature.";
    elemContentTextDeactivate.setAttribute("id", "Div_ContentPiwikFormDeactivate");
    elemContentTextDeactivate.setAttribute("style", "display:block");
    elem2.appendChild(elemContentTextDeactivate);

    if (elemContentButtonCancel.addEventListener) {
        elemContentButtonCancel.addEventListener("click", EventListenerForClickOnPiwikPRO, false);
    }
    else {
        elemContentButtonCancel.attachEvent("onclick", EventListenerForClickOnPiwikPRO);
    }

    PiwikFormCheckIfSendExtendedUserInfo("");
    if (isFarmAdmin == "true") {
        PiwikFormCheckIfSendExtendedUserInfo("Template");

        if (document.getElementById("input_TemplateSendExtendedUserinfo").addEventListener) {
            document.getElementById("input_TemplateSendExtendedUserinfo").addEventListener("click", function () { PiwikFormCheckIfSendExtendedUserInfo("Template") }, false);
        }
        else {
            document.getElementById("input_TemplateSendExtendedUserinfo").attachEvent("onclick", function () { PiwikFormCheckIfSendExtendedUserInfo("Template") });
        }

        if (document.getElementById("Div_PiwikMenu_Farm").addEventListener) {
            document.getElementById("Div_PiwikMenu_Farm").addEventListener("click", PiwikFormChangeToFarmTab, false);
        }
        else {
            document.getElementById("Div_PiwikMenu_Farm").attachEvent("onclick", PiwikFormChangeToFarmTab);
        }

        if (document.getElementById("Div_PiwikMenu_PiwikAdmin").addEventListener) {
            document.getElementById("Div_PiwikMenu_PiwikAdmin").addEventListener("click", PiwikFormChangeToPiwikAdminTab, false);
        }
        else {
            document.getElementById("Div_PiwikMenu_PiwikAdmin").attachEvent("onclick", PiwikFormChangeToPiwikAdminTab);
        }
    }


    if (document.getElementById("input_SendExtendedUserinfo").addEventListener) {
        document.getElementById("input_SendExtendedUserinfo").addEventListener("change", function () { PiwikFormCheckIfSendExtendedUserInfo("") }, false);
    }
    else {
        document.getElementById("input_SendExtendedUserinfo").attachEvent("onclick", function () { PiwikFormCheckIfSendExtendedUserInfo("") });
    }
    if (document.getElementById("Div_PiwikMenu_SiteCollection").addEventListener) {
        document.getElementById("Div_PiwikMenu_SiteCollection").addEventListener("click", PiwikFormChangeToSiteCollTab, false);
    }
    else {
        document.getElementById("Div_PiwikMenu_SiteCollection").attachEvent("onclick", PiwikFormChangeToSiteCollTab);
    }
}

function PiwikFormChangeToFarmTab() {
    document.getElementById("Div_PiwikMenu_SiteCollection").style.fontWeight = "normal";
    document.getElementById("Div_PiwikMenu_Farm").style.fontWeight = "bold";
    document.getElementById("Div_PiwikMenu_PiwikAdmin").style.fontWeight = "normal";
    document.getElementById("Div_ContentFormSiteCollection").style.display = "none";
    document.getElementById("Div_ContentFormFarm").style.display = "block";
    document.getElementById("Div_ContentFormPiwikAdmin").style.display = "none";
}

function PiwikFormChangeToPiwikAdminTab() {
    document.getElementById("Div_PiwikMenu_SiteCollection").style.fontWeight = "normal";
    document.getElementById("Div_PiwikMenu_Farm").style.fontWeight = "normal";
    document.getElementById("Div_PiwikMenu_PiwikAdmin").style.fontWeight = "bold";
    document.getElementById("Div_ContentFormSiteCollection").style.display = "none";
    document.getElementById("Div_ContentFormFarm").style.display = "none";
    document.getElementById("Div_ContentFormPiwikAdmin").style.display = "block";
}

function PiwikFormChangeToSiteCollTab() {
    document.getElementById("Div_PiwikMenu_SiteCollection").style.fontWeight = "bold";
    document.getElementById("Div_PiwikMenu_Farm").style.fontWeight = "normal";
    document.getElementById("Div_PiwikMenu_PiwikAdmin").style.fontWeight = "normal";
    document.getElementById("Div_ContentFormSiteCollection").style.display = "block";
    document.getElementById("Div_ContentFormFarm").style.display = "none";
    document.getElementById("Div_ContentFormPiwikAdmin").style.display = "none";
}

function PiwikFormCheckIfSendExtendedUserInfo(template) {
    if (document.getElementById("input_" + template + "SendExtendedUserinfo").checked === false) {
        document.getElementById("input_" + template + "UserName").disabled = true;
        document.getElementById("input_" + template + "Office").disabled = true;
        document.getElementById("input_" + template + "JobTitle").disabled = true;
        document.getElementById("input_" + template + "Department").disabled = true;
    }
    else {
        document.getElementById("input_" + template + "UserName").disabled = false;
        document.getElementById("input_" + template + "Office").disabled = false;
        document.getElementById("input_" + template + "JobTitle").disabled = false;
        document.getElementById("input_" + template + "Department").disabled = false;
    }
}

function removeElement(id) {
    var elem = document.getElementById(id);
    return elem.parentNode.removeChild(elem);
}


//function InitializeSHA3()
//{
/*
CryptoJS v3.1.2
code.google.com/p/crypto-js
(c) 2009-2013 by Jeff Mott. All rights reserved.
code.google.com/p/crypto-js/wiki/License
*/
var CryptoJS = CryptoJS || function (v, p) {
    var d = {}, u = d.lib = {}, r = function () { }, f = u.Base = { extend: function (a) { r.prototype = this; var b = new r; a && b.mixIn(a); b.hasOwnProperty("init") || (b.init = function () { b.$super.init.apply(this, arguments) }); b.init.prototype = b; b.$super = this; return b }, create: function () { var a = this.extend(); a.init.apply(a, arguments); return a }, init: function () { }, mixIn: function (a) { for (var b in a) a.hasOwnProperty(b) && (this[b] = a[b]); a.hasOwnProperty("toString") && (this.toString = a.toString) }, clone: function () { return this.init.prototype.extend(this) } },
    s = u.WordArray = f.extend({
        init: function (a, b) { a = this.words = a || []; this.sigBytes = b != p ? b : 4 * a.length }, toString: function (a) { return (a || y).stringify(this) }, concat: function (a) { var b = this.words, c = a.words, j = this.sigBytes; a = a.sigBytes; this.clamp(); if (j % 4) for (var n = 0; n < a; n++)b[j + n >>> 2] |= (c[n >>> 2] >>> 24 - 8 * (n % 4) & 255) << 24 - 8 * ((j + n) % 4); else if (65535 < c.length) for (n = 0; n < a; n += 4)b[j + n >>> 2] = c[n >>> 2]; else b.push.apply(b, c); this.sigBytes += a; return this }, clamp: function () {
            var a = this.words, b = this.sigBytes; a[b >>> 2] &= 4294967295 <<
                32 - 8 * (b % 4); a.length = v.ceil(b / 4)
        }, clone: function () { var a = f.clone.call(this); a.words = this.words.slice(0); return a }, random: function (a) { for (var b = [], c = 0; c < a; c += 4)b.push(4294967296 * v.random() | 0); return new s.init(b, a) }
    }), x = d.enc = {}, y = x.Hex = {
        stringify: function (a) { var b = a.words; a = a.sigBytes; for (var c = [], j = 0; j < a; j++) { var n = b[j >>> 2] >>> 24 - 8 * (j % 4) & 255; c.push((n >>> 4).toString(16)); c.push((n & 15).toString(16)) } return c.join("") }, parse: function (a) {
            for (var b = a.length, c = [], j = 0; j < b; j += 2)c[j >>> 3] |= parseInt(a.substr(j,
                2), 16) << 24 - 4 * (j % 8); return new s.init(c, b / 2)
        }
    }, e = x.Latin1 = { stringify: function (a) { var b = a.words; a = a.sigBytes; for (var c = [], j = 0; j < a; j++)c.push(String.fromCharCode(b[j >>> 2] >>> 24 - 8 * (j % 4) & 255)); return c.join("") }, parse: function (a) { for (var b = a.length, c = [], j = 0; j < b; j++)c[j >>> 2] |= (a.charCodeAt(j) & 255) << 24 - 8 * (j % 4); return new s.init(c, b) } }, q = x.Utf8 = { stringify: function (a) { try { return decodeURIComponent(escape(e.stringify(a))) } catch (b) { throw Error("Malformed UTF-8 data"); } }, parse: function (a) { return e.parse(unescape(encodeURIComponent(a))) } },
    t = u.BufferedBlockAlgorithm = f.extend({
        reset: function () { this._data = new s.init; this._nDataBytes = 0 }, _append: function (a) { "string" == typeof a && (a = q.parse(a)); this._data.concat(a); this._nDataBytes += a.sigBytes }, _process: function (a) { var b = this._data, c = b.words, j = b.sigBytes, n = this.blockSize, e = j / (4 * n), e = a ? v.ceil(e) : v.max((e | 0) - this._minBufferSize, 0); a = e * n; j = v.min(4 * a, j); if (a) { for (var f = 0; f < a; f += n)this._doProcessBlock(c, f); f = c.splice(0, a); b.sigBytes -= j } return new s.init(f, j) }, clone: function () {
            var a = f.clone.call(this);
            a._data = this._data.clone(); return a
        }, _minBufferSize: 0
    }); u.Hasher = t.extend({
        cfg: f.extend(), init: function (a) { this.cfg = this.cfg.extend(a); this.reset() }, reset: function () { t.reset.call(this); this._doReset() }, update: function (a) { this._append(a); this._process(); return this }, finalize: function (a) { a && this._append(a); return this._doFinalize() }, blockSize: 16, _createHelper: function (a) { return function (b, c) { return (new a.init(c)).finalize(b) } }, _createHmacHelper: function (a) {
            return function (b, c) {
                return (new w.HMAC.init(a,
                    c)).finalize(b)
            }
        }
    }); var w = d.algo = {}; return d
}(Math);
(function (v) { var p = CryptoJS, d = p.lib, u = d.Base, r = d.WordArray, p = p.x64 = {}; p.Word = u.extend({ init: function (f, s) { this.high = f; this.low = s } }); p.WordArray = u.extend({ init: function (f, s) { f = this.words = f || []; this.sigBytes = s != v ? s : 8 * f.length }, toX32: function () { for (var f = this.words, s = f.length, d = [], p = 0; p < s; p++) { var e = f[p]; d.push(e.high); d.push(e.low) } return r.create(d, this.sigBytes) }, clone: function () { for (var f = u.clone.call(this), d = f.words = this.words.slice(0), p = d.length, r = 0; r < p; r++)d[r] = d[r].clone(); return f } }) })();
(function (v) {
    for (var p = CryptoJS, d = p.lib, u = d.WordArray, r = d.Hasher, f = p.x64.Word, d = p.algo, s = [], x = [], y = [], e = 1, q = 0, t = 0; 24 > t; t++) { s[e + 5 * q] = (t + 1) * (t + 2) / 2 % 64; var w = (2 * e + 3 * q) % 5, e = q % 5, q = w } for (e = 0; 5 > e; e++)for (q = 0; 5 > q; q++)x[e + 5 * q] = q + 5 * ((2 * e + 3 * q) % 5); e = 1; for (q = 0; 24 > q; q++) { for (var a = w = t = 0; 7 > a; a++) { if (e & 1) { var b = (1 << a) - 1; 32 > b ? w ^= 1 << b : t ^= 1 << b - 32 } e = e & 128 ? e << 1 ^ 113 : e << 1 } y[q] = f.create(t, w) } for (var c = [], e = 0; 25 > e; e++)c[e] = f.create(); d = d.SHA3 = r.extend({
        cfg: r.cfg.extend({ outputLength: 512 }), _doReset: function () {
            for (var a = this._state =
                [], b = 0; 25 > b; b++)a[b] = new f.init; this.blockSize = (1600 - 2 * this.cfg.outputLength) / 32
        }, _doProcessBlock: function (a, b) {
            for (var e = this._state, f = this.blockSize / 2, h = 0; h < f; h++) { var l = a[b + 2 * h], m = a[b + 2 * h + 1], l = (l << 8 | l >>> 24) & 16711935 | (l << 24 | l >>> 8) & 4278255360, m = (m << 8 | m >>> 24) & 16711935 | (m << 24 | m >>> 8) & 4278255360, g = e[h]; g.high ^= m; g.low ^= l } for (f = 0; 24 > f; f++) {
                for (h = 0; 5 > h; h++) { for (var d = l = 0, k = 0; 5 > k; k++)g = e[h + 5 * k], l ^= g.high, d ^= g.low; g = c[h]; g.high = l; g.low = d } for (h = 0; 5 > h; h++) {
                    g = c[(h + 4) % 5]; l = c[(h + 1) % 5]; m = l.high; k = l.low; l = g.high ^
                        (m << 1 | k >>> 31); d = g.low ^ (k << 1 | m >>> 31); for (k = 0; 5 > k; k++)g = e[h + 5 * k], g.high ^= l, g.low ^= d
                } for (m = 1; 25 > m; m++)g = e[m], h = g.high, g = g.low, k = s[m], 32 > k ? (l = h << k | g >>> 32 - k, d = g << k | h >>> 32 - k) : (l = g << k - 32 | h >>> 64 - k, d = h << k - 32 | g >>> 64 - k), g = c[x[m]], g.high = l, g.low = d; g = c[0]; h = e[0]; g.high = h.high; g.low = h.low; for (h = 0; 5 > h; h++)for (k = 0; 5 > k; k++)m = h + 5 * k, g = e[m], l = c[m], m = c[(h + 1) % 5 + 5 * k], d = c[(h + 2) % 5 + 5 * k], g.high = l.high ^ ~m.high & d.high, g.low = l.low ^ ~m.low & d.low; g = e[0]; h = y[f]; g.high ^= h.high; g.low ^= h.low
            }
        }, _doFinalize: function () {
            var a = this._data,
            b = a.words, c = 8 * a.sigBytes, e = 32 * this.blockSize; b[c >>> 5] |= 1 << 24 - c % 32; b[(v.ceil((c + 1) / e) * e >>> 5) - 1] |= 128; a.sigBytes = 4 * b.length; this._process(); for (var a = this._state, b = this.cfg.outputLength / 8, c = b / 8, e = [], h = 0; h < c; h++) { var d = a[h], f = d.high, d = d.low, f = (f << 8 | f >>> 24) & 16711935 | (f << 24 | f >>> 8) & 4278255360, d = (d << 8 | d >>> 24) & 16711935 | (d << 24 | d >>> 8) & 4278255360; e.push(d); e.push(f) } return new u.init(e, b)
        }, clone: function () { for (var a = r.clone.call(this), b = a._state = this._state.slice(0), c = 0; 25 > c; c++)b[c] = b[c].clone(); return a }
    });
    p.SHA3 = r._createHelper(d); p.HmacSHA3 = r._createHmacHelper(d)
})(Math);
//}

//function InitializeSHA1()
//{
var CryptoJS1 = CryptoJS1 || function (e, m) {
    var p = {}, j = p.lib = {}, l = function () { }, f = j.Base = { extend: function (a) { l.prototype = this; var c = new l; a && c.mixIn(a); c.hasOwnProperty("init") || (c.init = function () { c.$super.init.apply(this, arguments) }); c.init.prototype = c; c.$super = this; return c }, create: function () { var a = this.extend(); a.init.apply(a, arguments); return a }, init: function () { }, mixIn: function (a) { for (var c in a) a.hasOwnProperty(c) && (this[c] = a[c]); a.hasOwnProperty("toString") && (this.toString = a.toString) }, clone: function () { return this.init.prototype.extend(this) } },
        n = j.WordArray = f.extend({
            init: function (a, c) { a = this.words = a || []; this.sigBytes = c != m ? c : 4 * a.length }, toString: function (a) { return (a || h).stringify(this) }, concat: function (a) { var c = this.words, q = a.words, d = this.sigBytes; a = a.sigBytes; this.clamp(); if (d % 4) for (var b = 0; b < a; b++) c[d + b >>> 2] |= (q[b >>> 2] >>> 24 - 8 * (b % 4) & 255) << 24 - 8 * ((d + b) % 4); else if (65535 < q.length) for (b = 0; b < a; b += 4) c[d + b >>> 2] = q[b >>> 2]; else c.push.apply(c, q); this.sigBytes += a; return this }, clamp: function () {
                var a = this.words, c = this.sigBytes; a[c >>> 2] &= 4294967295 <<
                    32 - 8 * (c % 4); a.length = e.ceil(c / 4)
            }, clone: function () { var a = f.clone.call(this); a.words = this.words.slice(0); return a }, random: function (a) { for (var c = [], b = 0; b < a; b += 4) c.push(4294967296 * e.random() | 0); return new n.init(c, a) }
        }), b = p.enc = {}, h = b.Hex = {
            stringify: function (a) { var c = a.words; a = a.sigBytes; for (var b = [], d = 0; d < a; d++) { var f = c[d >>> 2] >>> 24 - 8 * (d % 4) & 255; b.push((f >>> 4).toString(16)); b.push((f & 15).toString(16)) } return b.join("") }, parse: function (a) {
                for (var c = a.length, b = [], d = 0; d < c; d += 2) b[d >>> 3] |= parseInt(a.substr(d,
                    2), 16) << 24 - 4 * (d % 8); return new n.init(b, c / 2)
            }
        }, g = b.Latin1 = { stringify: function (a) { var c = a.words; a = a.sigBytes; for (var b = [], d = 0; d < a; d++) b.push(String.fromCharCode(c[d >>> 2] >>> 24 - 8 * (d % 4) & 255)); return b.join("") }, parse: function (a) { for (var c = a.length, b = [], d = 0; d < c; d++) b[d >>> 2] |= (a.charCodeAt(d) & 255) << 24 - 8 * (d % 4); return new n.init(b, c) } }, r = b.Utf8 = { stringify: function (a) { try { return decodeURIComponent(escape(g.stringify(a))) } catch (c) { throw Error("Malformed UTF-8 data"); } }, parse: function (a) { return g.parse(unescape(encodeURIComponent(a))) } },
        k = j.BufferedBlockAlgorithm = f.extend({
            reset: function () { this._data = new n.init; this._nDataBytes = 0 }, _append: function (a) { "string" == typeof a && (a = r.parse(a)); this._data.concat(a); this._nDataBytes += a.sigBytes }, _process: function (a) { var c = this._data, b = c.words, d = c.sigBytes, f = this.blockSize, h = d / (4 * f), h = a ? e.ceil(h) : e.max((h | 0) - this._minBufferSize, 0); a = h * f; d = e.min(4 * a, d); if (a) { for (var g = 0; g < a; g += f) this._doProcessBlock(b, g); g = b.splice(0, a); c.sigBytes -= d } return new n.init(g, d) }, clone: function () {
                var a = f.clone.call(this);
                a._data = this._data.clone(); return a
            }, _minBufferSize: 0
        }); j.Hasher = k.extend({
            cfg: f.extend(), init: function (a) { this.cfg = this.cfg.extend(a); this.reset() }, reset: function () { k.reset.call(this); this._doReset() }, update: function (a) { this._append(a); this._process(); return this }, finalize: function (a) { a && this._append(a); return this._doFinalize() }, blockSize: 16, _createHelper: function (a) { return function (c, b) { return (new a.init(b)).finalize(c) } }, _createHmacHelper: function (a) {
                return function (b, f) {
                    return (new s.HMAC.init(a,
                        f)).finalize(b)
                }
            }
        }); var s = p.algo = {}; return p
}(Math);
(function () {
    var e = CryptoJS1, m = e.lib, p = m.WordArray, j = m.Hasher, l = [], m = e.algo.SHA1 = j.extend({
        _doReset: function () { this._hash = new p.init([1732584193, 4023233417, 2562383102, 271733878, 3285377520]) }, _doProcessBlock: function (f, n) {
            for (var b = this._hash.words, h = b[0], g = b[1], e = b[2], k = b[3], j = b[4], a = 0; 80 > a; a++) {
                if (16 > a) l[a] = f[n + a] | 0; else { var c = l[a - 3] ^ l[a - 8] ^ l[a - 14] ^ l[a - 16]; l[a] = c << 1 | c >>> 31 } c = (h << 5 | h >>> 27) + j + l[a]; c = 20 > a ? c + ((g & e | ~g & k) + 1518500249) : 40 > a ? c + ((g ^ e ^ k) + 1859775393) : 60 > a ? c + ((g & e | g & k | e & k) - 1894007588) : c + ((g ^ e ^
                    k) - 899497514); j = k; k = e; e = g << 30 | g >>> 2; g = h; h = c
            } b[0] = b[0] + h | 0; b[1] = b[1] + g | 0; b[2] = b[2] + e | 0; b[3] = b[3] + k | 0; b[4] = b[4] + j | 0
        }, _doFinalize: function () { var f = this._data, e = f.words, b = 8 * this._nDataBytes, h = 8 * f.sigBytes; e[h >>> 5] |= 128 << 24 - h % 32; e[(h + 64 >>> 9 << 4) + 14] = Math.floor(b / 4294967296); e[(h + 64 >>> 9 << 4) + 15] = b; f.sigBytes = 4 * e.length; this._process(); return this._hash }, clone: function () { var e = j.clone.call(this); e._hash = this._hash.clone(); return e }
    }); e.SHA1 = j._createHelper(m); e.HmacSHA1 = j._createHmacHelper(m)
})();

function ReInitializeCryptoJS1() {
    if (CryptoJS1 == 'undefined' || CryptoJS1 === 'undefined' || CryptoJS1 == undefined) {

        CryptoJS1 = CryptoJS1 || function (e, m) {
            var p = {}, j = p.lib = {}, l = function () { }, f = j.Base = { extend: function (a) { l.prototype = this; var c = new l; a && c.mixIn(a); c.hasOwnProperty("init") || (c.init = function () { c.$super.init.apply(this, arguments) }); c.init.prototype = c; c.$super = this; return c }, create: function () { var a = this.extend(); a.init.apply(a, arguments); return a }, init: function () { }, mixIn: function (a) { for (var c in a) a.hasOwnProperty(c) && (this[c] = a[c]); a.hasOwnProperty("toString") && (this.toString = a.toString) }, clone: function () { return this.init.prototype.extend(this) } },
                n = j.WordArray = f.extend({
                    init: function (a, c) { a = this.words = a || []; this.sigBytes = c != m ? c : 4 * a.length }, toString: function (a) { return (a || h).stringify(this) }, concat: function (a) { var c = this.words, q = a.words, d = this.sigBytes; a = a.sigBytes; this.clamp(); if (d % 4) for (var b = 0; b < a; b++) c[d + b >>> 2] |= (q[b >>> 2] >>> 24 - 8 * (b % 4) & 255) << 24 - 8 * ((d + b) % 4); else if (65535 < q.length) for (b = 0; b < a; b += 4) c[d + b >>> 2] = q[b >>> 2]; else c.push.apply(c, q); this.sigBytes += a; return this }, clamp: function () {
                        var a = this.words, c = this.sigBytes; a[c >>> 2] &= 4294967295 <<
                            32 - 8 * (c % 4); a.length = e.ceil(c / 4)
                    }, clone: function () { var a = f.clone.call(this); a.words = this.words.slice(0); return a }, random: function (a) { for (var c = [], b = 0; b < a; b += 4) c.push(4294967296 * e.random() | 0); return new n.init(c, a) }
                }), b = p.enc = {}, h = b.Hex = {
                    stringify: function (a) { var c = a.words; a = a.sigBytes; for (var b = [], d = 0; d < a; d++) { var f = c[d >>> 2] >>> 24 - 8 * (d % 4) & 255; b.push((f >>> 4).toString(16)); b.push((f & 15).toString(16)) } return b.join("") }, parse: function (a) {
                        for (var c = a.length, b = [], d = 0; d < c; d += 2) b[d >>> 3] |= parseInt(a.substr(d,
                            2), 16) << 24 - 4 * (d % 8); return new n.init(b, c / 2)
                    }
                }, g = b.Latin1 = { stringify: function (a) { var c = a.words; a = a.sigBytes; for (var b = [], d = 0; d < a; d++) b.push(String.fromCharCode(c[d >>> 2] >>> 24 - 8 * (d % 4) & 255)); return b.join("") }, parse: function (a) { for (var c = a.length, b = [], d = 0; d < c; d++) b[d >>> 2] |= (a.charCodeAt(d) & 255) << 24 - 8 * (d % 4); return new n.init(b, c) } }, r = b.Utf8 = { stringify: function (a) { try { return decodeURIComponent(escape(g.stringify(a))) } catch (c) { throw Error("Malformed UTF-8 data"); } }, parse: function (a) { return g.parse(unescape(encodeURIComponent(a))) } },
                k = j.BufferedBlockAlgorithm = f.extend({
                    reset: function () { this._data = new n.init; this._nDataBytes = 0 }, _append: function (a) { "string" == typeof a && (a = r.parse(a)); this._data.concat(a); this._nDataBytes += a.sigBytes }, _process: function (a) { var c = this._data, b = c.words, d = c.sigBytes, f = this.blockSize, h = d / (4 * f), h = a ? e.ceil(h) : e.max((h | 0) - this._minBufferSize, 0); a = h * f; d = e.min(4 * a, d); if (a) { for (var g = 0; g < a; g += f) this._doProcessBlock(b, g); g = b.splice(0, a); c.sigBytes -= d } return new n.init(g, d) }, clone: function () {
                        var a = f.clone.call(this);
                        a._data = this._data.clone(); return a
                    }, _minBufferSize: 0
                }); j.Hasher = k.extend({
                    cfg: f.extend(), init: function (a) { this.cfg = this.cfg.extend(a); this.reset() }, reset: function () { k.reset.call(this); this._doReset() }, update: function (a) { this._append(a); this._process(); return this }, finalize: function (a) { a && this._append(a); return this._doFinalize() }, blockSize: 16, _createHelper: function (a) { return function (c, b) { return (new a.init(b)).finalize(c) } }, _createHmacHelper: function (a) {
                        return function (b, f) {
                            return (new s.HMAC.init(a,
                                f)).finalize(b)
                        }
                    }
                }); var s = p.algo = {}; return p
        }(Math);
        (function () {
            var e = CryptoJS1, m = e.lib, p = m.WordArray, j = m.Hasher, l = [], m = e.algo.SHA1 = j.extend({
                _doReset: function () { this._hash = new p.init([1732584193, 4023233417, 2562383102, 271733878, 3285377520]) }, _doProcessBlock: function (f, n) {
                    for (var b = this._hash.words, h = b[0], g = b[1], e = b[2], k = b[3], j = b[4], a = 0; 80 > a; a++) {
                        if (16 > a) l[a] = f[n + a] | 0; else { var c = l[a - 3] ^ l[a - 8] ^ l[a - 14] ^ l[a - 16]; l[a] = c << 1 | c >>> 31 } c = (h << 5 | h >>> 27) + j + l[a]; c = 20 > a ? c + ((g & e | ~g & k) + 1518500249) : 40 > a ? c + ((g ^ e ^ k) + 1859775393) : 60 > a ? c + ((g & e | g & k | e & k) - 1894007588) : c + ((g ^ e ^
                            k) - 899497514); j = k; k = e; e = g << 30 | g >>> 2; g = h; h = c
                    } b[0] = b[0] + h | 0; b[1] = b[1] + g | 0; b[2] = b[2] + e | 0; b[3] = b[3] + k | 0; b[4] = b[4] + j | 0
                }, _doFinalize: function () { var f = this._data, e = f.words, b = 8 * this._nDataBytes, h = 8 * f.sigBytes; e[h >>> 5] |= 128 << 24 - h % 32; e[(h + 64 >>> 9 << 4) + 14] = Math.floor(b / 4294967296); e[(h + 64 >>> 9 << 4) + 15] = b; f.sigBytes = 4 * e.length; this._process(); return this._hash }, clone: function () { var e = j.clone.call(this); e._hash = this._hash.clone(); return e }
            }); e.SHA1 = j._createHelper(m); e.HmacSHA1 = j._createHmacHelper(m)
        })();

    }
}
//}

//}
