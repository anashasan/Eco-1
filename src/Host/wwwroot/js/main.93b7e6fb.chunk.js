(window.webpackJsonp=window.webpackJsonp||[]).push([[0],{38:function(e,t,a){e.exports=a(76)},43:function(e,t,a){},76:function(e,t,a){"use strict";a.r(t);var n=a(1),o=a.n(n),r=a(10),l=a.n(r),c=(a(43),a(13)),i=a(9),s=a(11),m=a(31),d=a(32),u=a(36),h=a(33),f=a(37),p=a(3),g=a(12),v=a.n(g),E=a(78),b=a(79),y=a(80),D=a(81),S=a(82),k=a(83),w=a(84),I=a(85),C=a(86),j=a(87),O=a(88),N=a(89),P=a(93),A=a(90),F=a(91),x=a(92),L=a(17),Y=a.n(L),T=v.a.create({baseURL:"http://ecoservices.pk"}),M=a(14),W=a.n(M);function B(e,t,a){var r=e.dailyActivityPerformReport.map(function(t,n){return o.a.createElement("tr",{key:n},o.a.createElement("td",{rowSpan:"1",colSpan:"1"},t.stationNo),o.a.createElement("td",{rowSpan:"1",colSpan:"1"},t.locationName),function(e,t){var a=[];if(e)for(var n=function(n){var r=t[n],l=e.find(function(e){return e.activityName===r});l?a.push(o.a.createElement("td",{rowSpan:"1",colSpan:"1",key:n},l.perform)):a.push(o.a.createElement("td",{rowSpan:"1",colSpan:"1",key:n},"\xa0"))},r=0;r<=t.length-1;r++)n(r);else for(r=0;r<=t.length-1;r++)a.push(o.a.createElement("td",{rowSpan:"1",colSpan:"1",key:r},"\xa0"));return a}(t.activityPerform,e.activities),o.a.createElement("td",null,o.a.createElement(E.a,{type:"Button",onClick:a,value:"".concat(t.locationId,",").concat(t.stationNo)},"Edit")))});return o.a.createElement(n.Fragment,{key:t},o.a.createElement("h4",null,e.stationName),o.a.createElement(b.a,{className:"text-center",striped:!0,bordered:!0,responsive:!0},o.a.createElement("tbody",null,o.a.createElement("tr",{className:"table-secondary"},o.a.createElement("td",{rowSpan:"1",colSpan:"2"},"Station Details"),e.activities&&o.a.createElement("td",{rowSpan:"1",colSpan:e.activities.length+1},"Activity Perform")),o.a.createElement("tr",null,o.a.createElement("td",{rowSpan:"1",colSpan:"1"},"Station No"),o.a.createElement("td",{rowSpan:"1",colSpan:"1"},"Location Name"),e.activities&&e.activities.map(function(e,t){return o.a.createElement("td",{rowSpan:"1",colSpan:"1",key:t},e)}),o.a.createElement("td",null,"Action")),r)))}var U=function(e){function t(){var e;Object(m.a)(this,t),e=Object(u.a)(this,Object(h.a)(t).call(this));var a=W.a.parse(window.location.search.substring(1));return e.state={reports:[],fromDate:"",toDate:"",locationId:0,branchId:a.branchId,locations:[],modal:!1,formData:[],selectedLocationId:0,selectedSno:"",isLoading:!0},e.handleChange=e.handleChange.bind(Object(p.a)(Object(p.a)(e))),e.handleSubmit=e.handleSubmit.bind(Object(p.a)(Object(p.a)(e))),e.toggle=e.toggle.bind(Object(p.a)(Object(p.a)(e))),e.submitForm=e.submitForm.bind(Object(p.a)(Object(p.a)(e))),e.handleFormChange=e.handleFormChange.bind(Object(p.a)(Object(p.a)(e))),e.loadData=e.loadData.bind(Object(p.a)(Object(p.a)(e))),e}return Object(f.a)(t,e),Object(d.a)(t,[{key:"componentDidMount",value:function(){this.loadData()}},{key:"loadData",value:function(){var e=this;v.a.all([T.get("/Company/data?branchId=".concat(this.state.branchId)),T.get("/Company/locations/branchId/".concat(this.state.branchId))]).then(v.a.spread(function(t,a){e.setState({reports:Object(s.a)(t.data),locations:Object(s.a)(a.data),isLoading:!1})}))}},{key:"toggle",value:function(e){var t=this,a={modal:!this.state.modal};if(!this.state.modal){var n=e.target.value.split(",");a.selectedLocationId=n[0],a.selectedSno=n[1]}this.setState(Object(i.a)({},a),function(){t.state.modal?t.loadEditForm(t.state.selectedLocationId,t.state.branchId,t.state.selectedSno):(t.setState({isLoading:!0}),t.loadData())})}},{key:"handleChange",value:function(e){var t=e.target.name;this.setState(Object(c.a)({},t,e.target.value))}},{key:"handleFormChange",value:function(e){var t=e.target.name,a=Object(s.a)(this.state.formData),n=a.findIndex(function(e){return e.pkActivityPerformDetailId==t.match(/\d+/)[0]});"performText"===t.replace(/[0-9]/g,"")?a[n].perform=e.target.value:"isPerform"===t.replace(/[0-9]/g,"")&&(a[n].isPerform=e.target.checked),this.setState({formData:a}),e.preventDefault()}},{key:"handleSubmit",value:function(e){var t=this,a=W.a.stringify({branchId:this.state.branchId,locationId:0===this.state.locationId?null:this.state.locationId,fromDate:Y()(this.state.fromDate).format("DD/MM/YYYY"),toDate:Y()(this.state.toDate).format("DD/MM/YYYY")});T.get("/Company/data?".concat(a)).then(function(e){t.setState({reports:Object(s.a)(e.data)})}),e.preventDefault()}},{key:"loadEditForm",value:function(e,t,a){var n=this;T.get("/Company/GetData?"+W.a.stringify({locationId:e,branchId:t,Sno:a})).then(function(e){n.setState({formData:Object(s.a)(e.data).map(function(e){return e.perform||(e.perform=""),e})})})}},{key:"submitForm",value:function(e){var t=this;T.post("/Company/UpdateData",Object(i.a)({},this.state.formData.find(function(t){return t.pkActivityPerformDetailId===e.target.value}))).then(function(){t.setState(function(e){return{modal:!e.modal}})})}},{key:"render",value:function(){var e=this,t=this.state.reports.map(function(t,a){return B(t,a,e.toggle)});return o.a.createElement(y.a,null,o.a.createElement("br",null),o.a.createElement(D.a,null,o.a.createElement(S.a,null,o.a.createElement(k.a,null,"Search Box"),o.a.createElement(w.a,null,o.a.createElement(I.a,{inline:!0,onSubmit:this.handleSubmit},o.a.createElement(C.a,{className:"mb-2 mr-sm-2 mb-sm-0"},o.a.createElement(j.a,{for:"fromDate",className:"mr-sm-2"},"Starting Date"),o.a.createElement(O.a,{type:"date",id:"fromDate",name:"fromDate",value:this.state.fromDate,onChange:this.handleChange})),o.a.createElement(C.a,{className:"mb-2 mr-sm-2 mb-sm-0"},o.a.createElement(j.a,{for:"toDate",className:"mr-sm-2"},"Ending Date"),o.a.createElement(O.a,{type:"date",id:"toDate",name:"toDate",value:this.state.toDate,onChange:this.handleChange})),o.a.createElement(C.a,{className:"mb-2 mr-sm-2 mb-sm-0"},o.a.createElement(j.a,{for:"locationDropDown",className:"mr-sm-2"},"Location"),o.a.createElement(O.a,{type:"select",id:"locationDropDown",name:"locationId",value:this.state.locationId,onChange:this.handleChange},o.a.createElement("option",{value:null},"Select All"),this.state.locations.map(function(e,t){return o.a.createElement("option",{key:t,value:e.locationId},e.locationName)}))),o.a.createElement(E.a,null,"Search"))))),o.a.createElement("br",null),o.a.createElement(D.a,null,this.state.isLoading?o.a.createElement("div",{className:"d-flex justify-content-center"},o.a.createElement(N.a,{color:"dark"})):o.a.createElement("div",null,t)),o.a.createElement(D.a,null,o.a.createElement(P.a,{isOpen:this.state.modal,toggle:this.toggle,className:this.props.className},o.a.createElement(A.a,{toggle:this.toggle},"Edit"),o.a.createElement(F.a,null,o.a.createElement(b.a,{responsive:!0,striped:!0},o.a.createElement("thead",null,o.a.createElement("tr",null,o.a.createElement("th",null,"Activity"),o.a.createElement("th",null,"Date"),o.a.createElement("th",null,"Is Performed"),o.a.createElement("th",null,"Perform"),o.a.createElement("th",null,"Action"))),o.a.createElement("tbody",null,this.state.formData.map(function(t,a){var n=new Date(t.createdOn);return o.a.createElement("tr",{key:a},o.a.createElement("td",null,t.name),o.a.createElement("td",null,n.getDate(),"/",n.getMonth(),"/",n.getUTCFullYear()),o.a.createElement("td",{className:"text-center"},o.a.createElement("div",{className:"custom-control custom-checkbox mr-sm-2"},t.isPerform,o.a.createElement("input",{type:"checkbox",className:"custom-control-input",checked:t.isPerform,name:"isPerform"+t.pkActivityPerformDetailId,id:"performText"+t.pkActivityPerformDetailId+"id",onChange:e.handleFormChange}),o.a.createElement("label",{className:"custom-control-label",htmlFor:"performText"+t.pkActivityPerformDetailId+"id"}))),o.a.createElement("td",null,o.a.createElement(O.a,{type:"number",value:t.perform,name:"performText"+t.pkActivityPerformDetailId,onChange:e.handleFormChange})),o.a.createElement("td",null,o.a.createElement(E.a,{color:"primary",onClick:e.submitForm,value:t.pkActivityPerformDetailId},"Edit")))})))),o.a.createElement(x.a,null,o.a.createElement(E.a,{color:"secondary",onClick:this.toggle},"Close")))))}}]),t}(n.Component),R=Boolean("localhost"===window.location.hostname||"[::1]"===window.location.hostname||window.location.hostname.match(/^127(?:\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$/));function J(e){navigator.serviceWorker.register(e).then(function(e){e.onupdatefound=function(){var t=e.installing;t.onstatechange=function(){"installed"===t.state&&(navigator.serviceWorker.controller?console.log("New content is available; please refresh."):console.log("Content is cached for offline use."))}}}).catch(function(e){console.error("Error during service worker registration:",e)})}a(75);l.a.render(o.a.createElement(U,null),document.getElementById("root")),function(){if("serviceWorker"in navigator){if(new URL("",window.location).origin!==window.location.origin)return;window.addEventListener("load",function(){var e="".concat("","/service-worker.js");R?(function(e){fetch(e).then(function(t){404===t.status||-1===t.headers.get("content-type").indexOf("javascript")?navigator.serviceWorker.ready.then(function(e){e.unregister().then(function(){window.location.reload()})}):J(e)}).catch(function(){console.log("No internet connection found. App is running in offline mode.")})}(e),navigator.serviceWorker.ready.then(function(){console.log("This web app is being served cache-first by a service worker. To learn more, visit https://goo.gl/SC7cgQ")})):J(e)})}}()}},[[38,1,2]]]);
//# sourceMappingURL=main.93b7e6fb.chunk.js.map