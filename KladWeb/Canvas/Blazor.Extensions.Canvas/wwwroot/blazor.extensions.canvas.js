!function(t){var e={};function n(r){if(e[r])return e[r].exports;var i=e[r]={i:r,l:!1,exports:{}};return t[r].call(i.exports,i,i.exports,n),i.l=!0,i.exports}n.m=t,n.c=e,n.d=function(t,e,r){n.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:r})},n.r=function(t){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},n.t=function(t,e){if(1&e&&(t=n(t)),8&e)return t;if(4&e&&"object"==typeof t&&t&&t.__esModule)return t;var r=Object.create(null);if(n.r(r),Object.defineProperty(r,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var i in t)n.d(r,i,function(e){return t[e]}.bind(null,i));return r},n.n=function(t){var e=t&&t.__esModule?function(){return t.default}:function(){return t};return n.d(e,"a",e),e},n.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},n.p="",n(n.s=0)}([function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0});const r=n(1);var i;!function(t){const e={Canvas2d:new r.ContextManager("2d"),WebGL:new r.ContextManager("webgl")};t.initialize=function(){"undefined"==typeof window||window.BlazorExtensions?window.BlazorExtensions=Object.assign({},window.BlazorExtensions,e):window.BlazorExtensions=Object.assign({},e)}}(i||(i={})),i.initialize()},function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0});e.ContextManager=class{constructor(t){if(this.contexts=new Map,this.webGLObject=new Array,this.webGLContext=!1,this.webGLTypes=[WebGLBuffer,WebGLShader,WebGLProgram,WebGLFramebuffer,WebGLRenderbuffer,WebGLTexture,WebGLUniformLocation],this.add=(t,e)=>{if(!t)throw new Error("Invalid canvas.");if(!this.contexts.get(t.id)){var n;if(!(n=e?t.getContext(this.contextName,e):t.getContext(this.contextName)))throw new Error("Invalid context.");this.contexts.set(t.id,n)}},this.remove=t=>{this.contexts.delete(t.id)},this.setProperty=(t,e,n)=>{const r=this.getContext(t);this.setPropertyWithContext(r,e,n)},this.getProperty=(t,e)=>{const n=this.getContext(t);return this.serialize(n[e])},this.call=(t,e,n)=>{const r=this.getContext(t);return this.callWithContext(r,e,n)},this.callBatch=(t,e)=>{const n=this.getContext(t);for(let t=0;t<e.length;t++){let r=e[t].slice(2);e[t][1]?this.callWithContext(n,e[t][0],r):this.setPropertyWithContext(n,e[t][0],Array.isArray(r)&&r.length>0?r[0]:null)}},this.callWithContext=(t,e,n)=>this.serialize(this.prototypes[e].apply(t,null!=n?n.map(t=>this.deserialize(e,t)):[])),this.setPropertyWithContext=(t,e,n)=>{t[e]=this.deserialize(e,n)},this.getContext=t=>{if(!t)throw new Error("Invalid canvas.");const e=this.contexts.get(t.id);if(!e)throw new Error("Invalid context.");return e},this.deserialize=(t,e)=>{if(!this.webGLContext||null==e)return e;if(e.hasOwnProperty("webGLType")&&e.hasOwnProperty("id"))return this.webGLObject[e.id];if(Array.isArray(e)&&!t.endsWith("v"))return Int8Array.of(...e);if("string"!=typeof e||"bufferData"!==t&&"bufferSubData"!==t)return e;{let t=window.atob(e),r=t.length,i=new Uint8Array(r);for(var n=0;n<r;n++)i[n]=t.charCodeAt(n);return i}},this.serialize=t=>{if(t instanceof TextMetrics)return{width:t.width};if(!this.webGLContext||null==t)return t;const e=this.webGLTypes.find(e=>t instanceof e);if(null!=e){const n=this.webGLObject.length;return this.webGLObject.push(t),{webGLType:e.name,id:n}}return t},this.contextName=t,"2d"===t)this.prototypes=CanvasRenderingContext2D.prototype;else{if("webgl"!==t&&"experimental-webgl"!==t)throw new Error("Invalid context name: "+t);this.prototypes=WebGLRenderingContext.prototype,this.webGLContext=!0}}}}]);
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly8vd2VicGFjay9ib290c3RyYXAiLCJ3ZWJwYWNrOi8vLy4vc3JjL0luaXRpYWxpemVDYW52YXMudHMiLCJ3ZWJwYWNrOi8vLy4vc3JjL0NhbnZhc0NvbnRleHRNYW5hZ2VyLnRzIl0sIm5hbWVzIjpbImluc3RhbGxlZE1vZHVsZXMiLCJfX3dlYnBhY2tfcmVxdWlyZV9fIiwibW9kdWxlSWQiLCJleHBvcnRzIiwibW9kdWxlIiwiaSIsImwiLCJtb2R1bGVzIiwiY2FsbCIsIm0iLCJjIiwiZCIsIm5hbWUiLCJnZXR0ZXIiLCJvIiwiT2JqZWN0IiwiZGVmaW5lUHJvcGVydHkiLCJlbnVtZXJhYmxlIiwiZ2V0IiwiciIsIlN5bWJvbCIsInRvU3RyaW5nVGFnIiwidmFsdWUiLCJ0IiwibW9kZSIsIl9fZXNNb2R1bGUiLCJucyIsImNyZWF0ZSIsImtleSIsImJpbmQiLCJuIiwib2JqZWN0IiwicHJvcGVydHkiLCJwcm90b3R5cGUiLCJoYXNPd25Qcm9wZXJ0eSIsInAiLCJzIiwiQ2FudmFzIiwiZXh0ZW5zaW9uT2JqZWN0IiwiQ2FudmFzMmQiLCJDb250ZXh0TWFuYWdlciIsIldlYkdMIiwiaW5pdGlhbGl6ZSIsIndpbmRvdyIsImNvbnRleHROYW1lIiwiY29udGV4dHMiLCJNYXAiLCJ3ZWJHTE9iamVjdCIsIkFycmF5Iiwid2ViR0xDb250ZXh0Iiwid2ViR0xUeXBlcyIsIldlYkdMQnVmZmVyIiwiV2ViR0xTaGFkZXIiLCJXZWJHTFByb2dyYW0iLCJXZWJHTEZyYW1lYnVmZmVyIiwiV2ViR0xSZW5kZXJidWZmZXIiLCJXZWJHTFRleHR1cmUiLCJXZWJHTFVuaWZvcm1Mb2NhdGlvbiIsImFkZCIsImNhbnZhcyIsInBhcmFtZXRlcnMiLCJFcnJvciIsInRoaXMiLCJpZCIsImNvbnRleHQiLCJnZXRDb250ZXh0Iiwic2V0IiwicmVtb3ZlIiwiZGVsZXRlIiwic2V0UHJvcGVydHkiLCJzZXRQcm9wZXJ0eVdpdGhDb250ZXh0IiwiZ2V0UHJvcGVydHkiLCJzZXJpYWxpemUiLCJtZXRob2QiLCJhcmdzIiwiY2FsbFdpdGhDb250ZXh0IiwiY2FsbEJhdGNoIiwiYmF0Y2hlZENhbGxzIiwibGVuZ3RoIiwicGFyYW1zIiwic2xpY2UiLCJpc0FycmF5IiwicHJvdG90eXBlcyIsImFwcGx5IiwidW5kZWZpbmVkIiwibWFwIiwiZGVzZXJpYWxpemUiLCJlbmRzV2l0aCIsIkludDhBcnJheSIsIm9mIiwiYmluU3RyIiwiYXRvYiIsImJ5dGVzIiwiVWludDhBcnJheSIsImNoYXJDb2RlQXQiLCJUZXh0TWV0cmljcyIsIndpZHRoIiwidHlwZSIsImZpbmQiLCJwdXNoIiwid2ViR0xUeXBlIiwiQ2FudmFzUmVuZGVyaW5nQ29udGV4dDJEIiwiV2ViR0xSZW5kZXJpbmdDb250ZXh0Il0sIm1hcHBpbmdzIjoiYUFDRSxJQUFJQSxFQUFtQixHQUd2QixTQUFTQyxFQUFvQkMsR0FHNUIsR0FBR0YsRUFBaUJFLEdBQ25CLE9BQU9GLEVBQWlCRSxHQUFVQyxRQUduQyxJQUFJQyxFQUFTSixFQUFpQkUsR0FBWSxDQUN6Q0csRUFBR0gsRUFDSEksR0FBRyxFQUNISCxRQUFTLElBVVYsT0FOQUksRUFBUUwsR0FBVU0sS0FBS0osRUFBT0QsUUFBU0MsRUFBUUEsRUFBT0QsUUFBU0YsR0FHL0RHLEVBQU9FLEdBQUksRUFHSkYsRUFBT0QsUUFLZkYsRUFBb0JRLEVBQUlGLEVBR3hCTixFQUFvQlMsRUFBSVYsRUFHeEJDLEVBQW9CVSxFQUFJLFNBQVNSLEVBQVNTLEVBQU1DLEdBQzNDWixFQUFvQmEsRUFBRVgsRUFBU1MsSUFDbENHLE9BQU9DLGVBQWViLEVBQVNTLEVBQU0sQ0FBRUssWUFBWSxFQUFNQyxJQUFLTCxLQUtoRVosRUFBb0JrQixFQUFJLFNBQVNoQixHQUNYLG9CQUFYaUIsUUFBMEJBLE9BQU9DLGFBQzFDTixPQUFPQyxlQUFlYixFQUFTaUIsT0FBT0MsWUFBYSxDQUFFQyxNQUFPLFdBRTdEUCxPQUFPQyxlQUFlYixFQUFTLGFBQWMsQ0FBRW1CLE9BQU8sS0FRdkRyQixF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QmMsRUFBUSxJQUFJQyxXQUFXZixHQUMzQixJQUFLLElBQUl6RSxFQUFJLEVBQUdBLEVBQUl5RSxFQUFRekUsSUFDeEJ1RixFQUFNdkYsR0FBS3FGLEVBQU9JLFdBQVd6RixHQUVqQyxPQUFPdUYsSUFLSCxLQUFBcEIsVUFBYXpDLElBQ25CLEdBQUlBLGFBQWtCZ0UsWUFDbEIsTUFBTyxDQUFFQyxNQUFPakUsRUFBT2lFLE9BRzNCLElBQUtsQyxLQUFLYixjQUEwQm1DLE1BQVZyRCxFQUFxQixPQUFPQSxFQUV0RCxNQUFNa0UsRUFBT25DLEtBQUtaLFdBQVdnRCxLQUFNRCxHQUFTbEUsYUFBa0JrRSxHQUM5RCxHQUFZYixNQUFSYSxFQUFtQixDQUNyQixNQUFNbEMsRUFBS0QsS0FBS2YsWUFBWStCLE9BRzVCLE9BRkFoQixLQUFLZixZQUFZb0QsS0FBS3BFLEdBRWYsQ0FDTHFFLFVBQVdILEVBQUtyRixLQUNoQm1ELEdBQUlBLEdBR04sT0FBT2hDLEdBaEhUK0IsS0FBS2xCLFlBQWNBLEVBQ0MsT0FBaEJBLEVBQ0ZrQixLQUFLb0IsV0FBYW1CLHlCQUF5QnBFLGNBQ3hDLElBQW9CLFVBQWhCVyxHQUEyQyx1QkFBaEJBLEVBSWxDLE1BQU0sSUFBSWlCLE1BQU0seUJBQXlCakIsR0FIekNrQixLQUFLb0IsV0FBYW9CLHNCQUFzQnJFLFVBQ3hDNkIsS0FBS2IsY0FBZSIsImZpbGUiOiJibGF6b3IuZXh0ZW5zaW9ucy5jYW52YXMuanMiLCJzb3VyY2VzQ29udGVudCI6WyIgXHQvLyBUaGUgbW9kdWxlIGNhY2hlXG4gXHR2YXIgaW5zdGFsbGVkTW9kdWxlcyA9IHt9O1xuXG4gXHQvLyBUaGUgcmVxdWlyZSBmdW5jdGlvblxuIFx0ZnVuY3Rpb24gX193ZWJwYWNrX3JlcXVpcmVfXyhtb2R1bGVJZCkge1xuXG4gXHRcdC8vIENoZWNrIGlmIG1vZHVsZSBpcyBpbiBjYWNoZVxuIFx0XHRpZihpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXSkge1xuIFx0XHRcdHJldHVybiBpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXS5leHBvcnRzO1xuIFx0XHR9XG4gXHRcdC8vIENyZWF0ZSBhIG5ldyBtb2R1bGUgKGFuZCBwdXQgaXQgaW50byB0aGUgY2FjaGUpXG4gXHRcdHZhciBtb2R1bGUgPSBpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXSA9IHtcbiBcdFx0XHRpOiBtb2R1bGVJZCxcbiBcdFx0XHRsOiBmYWxzZSxcbiBcdFx0XHRleHBvcnRzOiB7fVxuIFx0XHR9O1xuXG4gXHRcdC8vIEV4ZWN1dGUgdGhlIG1vZHVsZSBmdW5jdGlvblxuIFx0XHRtb2R1bGVzW21vZHVsZUlkXS5jYWxsKG1vZHVsZS5leHBvcnRzLCBtb2R1bGUsIG1vZHVsZS5leHBvcnRzLCBfX3dlYnBhY2tfcmVxdWlyZV9fKTtcblxuIFx0XHQvLyBGbGFnIHRoZSBtb2R1bGUgYXMgbG9hZGVkXG4gXHRcdG1vZHVsZS5sID0gdHJ1ZTtcblxuIFx0XHQvLyBSZXR1cm4gdGhlIGV4cG9ydHMgb2YgdGhlIG1vZHVsZVxuIFx0XHRyZXR1cm4gbW9kdWxlLmV4cG9ydHM7XG4gXHR9XG5cblxuIFx0Ly8gZXhwb3NlIHRoZSBtb2R1bGVzIG9iamVjdCAoX193ZWJwYWNrX21vZHVsZXNfXylcbiBcdF9fd2VicGFja19yZXF1aXJlX18ubSA9IG1vZHVsZXM7XG5cbiBcdC8vIGV4cG9zZSB0aGUgbW9kdWxlIGNhY2hlXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLmMgPSBpbnN0YWxsZWRNb2R1bGVzO1xuXG4gXHQvLyBkZWZpbmUgZ2V0dGVyIGZ1bmN0aW9uIGZvciBoYXJtb255IGV4cG9ydHNcbiBcdF9fd2VicGFja19yZXF1aXJlX18uZCA9IGZ1bmN0aW9uKGV4cG9ydHMsIG5hbWUsIGdldHRlcikge1xuIFx0XHRpZighX193ZWJwYWNrX3JlcXVpcmVfXy5vKGV4cG9ydHMsIG5hbWUpKSB7XG4gXHRcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIG5hbWUsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBnZXR0ZXIgfSk7XG4gXHRcdH1cbiBcdH07XG5cbiBcdC8vIGRlZmluZSBfX2VzTW9kdWxlIG9uIGV4cG9ydHNcbiBcdF9fd2VicGFja19yZXF1aXJlX18uciA9IGZ1bmN0aW9uKGV4cG9ydHMpIHtcbiBcdFx0aWYodHlwZW9mIFN5bWJvbCAhPT0gJ3VuZGVmaW5lZCcgJiYgU3ltYm9sLnRvU3RyaW5nVGFnKSB7XG4gXHRcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFN5bWJvbC50b1N0cmluZ1RhZywgeyB2YWx1ZTogJ01vZHVsZScgfSk7XG4gXHRcdH1cbiBcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsICdfX2VzTW9kdWxlJywgeyB2YWx1ZTogdHJ1ZSB9KTtcbiBcdH07XG5cbiBcdC8vIGNyZWF0ZSBhIGZha2UgbmFtZXNwYWNlIG9iamVjdFxuIFx0Ly8gbW9kZSAmIDE6IHZhbHVlIGlzIGEgbW9kdWxlIGlkLCByZXF1aXJlIGl0XG4gXHQvLyBtb2RlICYgMjogbWVyZ2UgYWxsIHByb3BlcnRpZXMgb2YgdmFsdWUgaW50byB0aGUgbnNcbiBcdC8vIG1vZGUgJiA0OiByZXR1cm4gdmFsdWUgd2hlbiBhbHJlYWR5IG5zIG9iamVjdFxuIFx0Ly8gbW9kZSAmIDh8MTogYmVoYXZlIGxpa2UgcmVxdWlyZVxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy50ID0gZnVuY3Rpb24odmFsdWUsIG1vZGUpIHtcbiBcdFx0aWYobW9kZSAmIDEpIHZhbHVlID0gX193ZWJwYWNrX3JlcXVpcmVfXyh2YWx1ZSk7XG4gXHRcdGlmKG1vZGUgJiA4KSByZXR1cm4gdmFsdWU7XG4gXHRcdGlmKChtb2RlICYgNCkgJiYgdHlwZW9mIHZhbHVlID09PSAnb2JqZWN0JyAmJiB2YWx1ZSAmJiB2YWx1ZS5fX2VzTW9kdWxlKSByZXR1cm4gdmFsdWU7XG4gXHRcdHZhciBucyA9IE9iamVjdC5jcmVhdGUobnVsbCk7XG4gXHRcdF9fd2VicGFja19yZXF1aXJlX18ucihucyk7XG4gXHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShucywgJ2RlZmF1bHQnLCB7IGVudW1lcmFibGU6IHRydWUsIHZhbHVlOiB2YWx1ZSB9KTtcbiBcdFx0aWYobW9kZSAmIDIgJiYgdHlwZW9mIHZhbHVlICE9ICdzdHJpbmcnKSBmb3IodmFyIGtleSBpbiB2YWx1ZSkgX193ZWJwYWNrX3JlcXVpcmVfXy5kKG5zLCBrZXksIGZ1bmN0aW9uKGtleSkgeyByZXR1cm4gdmFsdWVba2V5XTsgfS5iaW5kKG51bGwsIGtleSkpO1xuIFx0XHRyZXR1cm4gbnM7XG4gXHR9O1xuXG4gXHQvLyBnZXREZWZhdWx0RXhwb3J0IGZ1bmN0aW9uIGZvciBjb21wYXRpYmlsaXR5IHdpdGggbm9uLWhhcm1vbnkgbW9kdWxlc1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5uID0gZnVuY3Rpb24obW9kdWxlKSB7XG4gXHRcdHZhciBnZXR0ZXIgPSBtb2R1bGUgJiYgbW9kdWxlLl9fZXNNb2R1bGUgP1xuIFx0XHRcdGZ1bmN0aW9uIGdldERlZmF1bHQoKSB7IHJldHVybiBtb2R1bGVbJ2RlZmF1bHQnXTsgfSA6XG4gXHRcdFx0ZnVuY3Rpb24gZ2V0TW9kdWxlRXhwb3J0cygpIHsgcmV0dXJuIG1vZHVsZTsgfTtcbiBcdFx0X193ZWJwYWNrX3JlcXVpcmVfXy5kKGdldHRlciwgJ2EnLCBnZXR0ZXIpO1xuIFx0XHRyZXR1cm4gZ2V0dGVyO1xuIFx0fTtcblxuIFx0Ly8gT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLm8gPSBmdW5jdGlvbihvYmplY3QsIHByb3BlcnR5KSB7IHJldHVybiBPYmplY3QucHJvdG90eXBlLmhhc093blByb3BlcnR5LmNhbGwob2JqZWN0LCBwcm9wZXJ0eSk7IH07XG5cbiBcdC8vIF9fd2VicGFja19wdWJsaWNfcGF0aF9fXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLnAgPSBcIlwiO1xuXG5cbiBcdC8vIExvYWQgZW50cnkgbW9kdWxlIGFuZCByZXR1cm4gZXhwb3J0c1xuIFx0cmV0dXJuIF9fd2VicGFja19yZXF1aXJlX18oX193ZWJwYWNrX3JlcXVpcmVfXy5zID0gMCk7XG4iLCJpbXBvcnQgeyBDb250ZXh0TWFuYWdlciB9IGZyb20gJy4vQ2FudmFzQ29udGV4dE1hbmFnZXInO1xuXG5uYW1lc3BhY2UgQ2FudmFzIHtcbiAgY29uc3QgYmxhem9yRXh0ZW5zaW9uczogc3RyaW5nID0gJ0JsYXpvckV4dGVuc2lvbnMnO1xuICAvLyBkZWZpbmUgd2hhdCB0aGlzIGV4dGVuc2lvbiBhZGRzIHRvIHRoZSB3aW5kb3cgb2JqZWN0IGluc2lkZSBCbGF6b3JFeHRlbnNpb25zXG4gIGNvbnN0IGV4dGVuc2lvbk9iamVjdCA9IHtcbiAgICBDYW52YXMyZDogbmV3IENvbnRleHRNYW5hZ2VyKFwiMmRcIiksXG4gICAgV2ViR0w6IG5ldyBDb250ZXh0TWFuYWdlcihcIndlYmdsXCIpXG4gIH07XG5cbiAgZXhwb3J0IGZ1bmN0aW9uIGluaXRpYWxpemUoKTogdm9pZCB7XG4gICAgaWYgKHR5cGVvZiB3aW5kb3cgIT09ICd1bmRlZmluZWQnICYmICF3aW5kb3dbYmxhem9yRXh0ZW5zaW9uc10pIHtcbiAgICAgIC8vIHdoZW4gdGhlIGxpYnJhcnkgaXMgbG9hZGVkIGluIGEgYnJvd3NlciB2aWEgYSA8c2NyaXB0PiBlbGVtZW50LCBtYWtlIHRoZVxuICAgICAgLy8gZm9sbG93aW5nIEFQSXMgYXZhaWxhYmxlIGluIGdsb2JhbCBzY29wZSBmb3IgaW52b2NhdGlvbiBmcm9tIEpTXG4gICAgICB3aW5kb3dbYmxhem9yRXh0ZW5zaW9uc10gPSB7XG4gICAgICAgIC4uLmV4dGVuc2lvbk9iamVjdFxuICAgICAgfTtcbiAgICB9IGVsc2Uge1xuICAgICAgd2luZG93W2JsYXpvckV4dGVuc2lvbnNdID0ge1xuICAgICAgICAuLi53aW5kb3dbYmxhem9yRXh0ZW5zaW9uc10sXG4gICAgICAgIC4uLmV4dGVuc2lvbk9iamVjdFxuICAgICAgfTtcbiAgICB9XG4gIH1cbn1cblxuQ2FudmFzLmluaXRpYWxpemUoKTtcbiIsImV4cG9ydCBjbGFzcyBDb250ZXh0TWFuYWdlciB7XG4gIHByaXZhdGUgcmVhZG9ubHkgY29udGV4dHMgPSBuZXcgTWFwPHN0cmluZywgYW55PigpO1xuICBwcml2YXRlIHJlYWRvbmx5IHdlYkdMT2JqZWN0ID0gbmV3IEFycmF5PGFueT4oKTtcbiAgcHJpdmF0ZSByZWFkb25seSBjb250ZXh0TmFtZTogc3RyaW5nO1xuICBwcml2YXRlIHdlYkdMQ29udGV4dCA9IGZhbHNlO1xuICBwcml2YXRlIHJlYWRvbmx5IHByb3RvdHlwZXM6IGFueTtcbiAgcHJpdmF0ZSByZWFkb25seSB3ZWJHTFR5cGVzID0gW1xuICAgIFdlYkdMQnVmZmVyLCBXZWJHTFNoYWRlciwgV2ViR0xQcm9ncmFtLCBXZWJHTEZyYW1lYnVmZmVyLCBXZWJHTFJlbmRlcmJ1ZmZlciwgV2ViR0xUZXh0dXJlLCBXZWJHTFVuaWZvcm1Mb2NhdGlvblxuICBdO1xuXG4gIHB1YmxpYyBjb25zdHJ1Y3Rvcihjb250ZXh0TmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb250ZXh0TmFtZSA9IGNvbnRleHROYW1lO1xuICAgIGlmIChjb250ZXh0TmFtZSA9PT0gXCIyZFwiKVxuICAgICAgdGhpcy5wcm90b3R5cGVzID0gQ2FudmFzUmVuZGVyaW5nQ29udGV4dDJELnByb3RvdHlwZTtcbiAgICBlbHNlIGlmIChjb250ZXh0TmFtZSA9PT0gXCJ3ZWJnbFwiIHx8IGNvbnRleHROYW1lID09PSBcImV4cGVyaW1lbnRhbC13ZWJnbFwiKSB7XG4gICAgICB0aGlzLnByb3RvdHlwZXMgPSBXZWJHTFJlbmRlcmluZ0NvbnRleHQucHJvdG90eXBlO1xuICAgICAgdGhpcy53ZWJHTENvbnRleHQgPSB0cnVlO1xuICAgIH0gZWxzZVxuICAgICAgdGhyb3cgbmV3IEVycm9yKGBJbnZhbGlkIGNvbnRleHQgbmFtZTogJHtjb250ZXh0TmFtZX1gKTtcbiAgfVxuXG4gIHB1YmxpYyBhZGQgPSAoY2FudmFzOiBIVE1MQ2FudmFzRWxlbWVudCwgcGFyYW1ldGVyczogYW55KSA9PiB7XG4gICAgaWYgKCFjYW52YXMpIHRocm93IG5ldyBFcnJvcignSW52YWxpZCBjYW52YXMuJyk7XG4gICAgaWYgKHRoaXMuY29udGV4dHMuZ2V0KGNhbnZhcy5pZCkpIHJldHVybjtcblxuICAgIHZhciBjb250ZXh0O1xuICAgIGlmIChwYXJhbWV0ZXJzKVxuICAgICAgY29udGV4dCA9IGNhbnZhcy5nZXRDb250ZXh0KHRoaXMuY29udGV4dE5hbWUsIHBhcmFtZXRlcnMpO1xuICAgIGVsc2VcbiAgICAgIGNvbnRleHQgPSBjYW52YXMuZ2V0Q29udGV4dCh0aGlzLmNvbnRleHROYW1lKTtcblxuICAgIGlmICghY29udGV4dCkgdGhyb3cgbmV3IEVycm9yKCdJbnZhbGlkIGNvbnRleHQuJyk7XG5cbiAgICB0aGlzLmNvbnRleHRzLnNldChjYW52YXMuaWQsIGNvbnRleHQpO1xuICB9XG5cbiAgcHVibGljIHJlbW92ZSA9IChjYW52YXM6IEhUTUxDYW52YXNFbGVtZW50KSA9PiB7XG4gICAgdGhpcy5jb250ZXh0cy5kZWxldGUoY2FudmFzLmlkKTtcbiAgfVxuXG4gIHB1YmxpYyBzZXRQcm9wZXJ0eSA9IChjYW52YXM6IEhUTUxDYW52YXNFbGVtZW50LCBwcm9wZXJ0eTogc3RyaW5nLCB2YWx1ZTogYW55KSA9PiB7XG4gICAgY29uc3QgY29udGV4dCA9IHRoaXMuZ2V0Q29udGV4dChjYW52YXMpO1xuICAgIHRoaXMuc2V0UHJvcGVydHlXaXRoQ29udGV4dChjb250ZXh0LCBwcm9wZXJ0eSwgdmFsdWUpO1xuICB9XG5cbiAgcHVibGljIGdldFByb3BlcnR5ID0gKGNhbnZhczogSFRNTENhbnZhc0VsZW1lbnQsIHByb3BlcnR5OiBzdHJpbmcpID0+IHtcbiAgICBjb25zdCBjb250ZXh0ID0gdGhpcy5nZXRDb250ZXh0KGNhbnZhcyk7XG4gICAgcmV0dXJuIHRoaXMuc2VyaWFsaXplKGNvbnRleHRbcHJvcGVydHldKTtcbiAgfVxuXG4gIHB1YmxpYyBjYWxsID0gKGNhbnZhczogSFRNTENhbnZhc0VsZW1lbnQsIG1ldGhvZDogc3RyaW5nLCBhcmdzOiBhbnkpID0+IHtcbiAgICBjb25zdCBjb250ZXh0ID0gdGhpcy5nZXRDb250ZXh0KGNhbnZhcyk7XG4gICAgcmV0dXJuIHRoaXMuY2FsbFdpdGhDb250ZXh0KGNvbnRleHQsIG1ldGhvZCwgYXJncyk7XG4gIH1cblxuICBwdWJsaWMgY2FsbEJhdGNoID0gKGNhbnZhczogSFRNTENhbnZhc0VsZW1lbnQsIGJhdGNoZWRDYWxsczogYW55W11bXSkgPT4ge1xuICAgIGNvbnN0IGNvbnRleHQgPSB0aGlzLmdldENvbnRleHQoY2FudmFzKTtcbiAgICBmb3IgKGxldCBpID0gMDsgaSA8IGJhdGNoZWRDYWxscy5sZW5ndGg7IGkrKykge1xuICAgICAgbGV0IHBhcmFtcyA9IGJhdGNoZWRDYWxsc1tpXS5zbGljZSgyKTtcbiAgICAgIGlmIChiYXRjaGVkQ2FsbHNbaV1bMV0pIHtcbiAgICAgICAgdGhpcy5jYWxsV2l0aENvbnRleHQoY29udGV4dCwgYmF0Y2hlZENhbGxzW2ldWzBdLCBwYXJhbXMpO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgdGhpcy5zZXRQcm9wZXJ0eVdpdGhDb250ZXh0KFxuICAgICAgICAgIGNvbnRleHQsXG4gICAgICAgICAgYmF0Y2hlZENhbGxzW2ldWzBdLFxuICAgICAgICAgIEFycmF5LmlzQXJyYXkocGFyYW1zKSAmJiBwYXJhbXMubGVuZ3RoID4gMCA/IHBhcmFtc1swXSA6IG51bGwpO1xuICAgICAgfVxuICAgIH1cbiAgfVxuXG4gIHByaXZhdGUgY2FsbFdpdGhDb250ZXh0ID0gKGNvbnRleHQ6IGFueSwgbWV0aG9kOiBzdHJpbmcsIGFyZ3M6IGFueSkgPT4ge1xuICAgIHJldHVybiB0aGlzLnNlcmlhbGl6ZSh0aGlzLnByb3RvdHlwZXNbbWV0aG9kXS5hcHBseShjb250ZXh0LCBhcmdzICE9IHVuZGVmaW5lZCA/IGFyZ3MubWFwKCh2YWx1ZSkgPT4gdGhpcy5kZXNlcmlhbGl6ZShtZXRob2QsIHZhbHVlKSkgOiBbXSkpO1xuICB9XG5cbiAgcHJpdmF0ZSBzZXRQcm9wZXJ0eVdpdGhDb250ZXh0ID0gKGNvbnRleHQ6IGFueSwgcHJvcGVydHk6IHN0cmluZywgdmFsdWU6IGFueSkgPT4ge1xuICAgIGNvbnRleHRbcHJvcGVydHldID0gdGhpcy5kZXNlcmlhbGl6ZShwcm9wZXJ0eSwgdmFsdWUpO1xuICB9XG5cbiAgcHJpdmF0ZSBnZXRDb250ZXh0ID0gKGNhbnZhczogSFRNTENhbnZhc0VsZW1lbnQpID0+IHtcbiAgICBpZiAoIWNhbnZhcykgdGhyb3cgbmV3IEVycm9yKCdJbnZhbGlkIGNhbnZhcy4nKTtcblxuICAgIGNvbnN0IGNvbnRleHQgPSB0aGlzLmNvbnRleHRzLmdldChjYW52YXMuaWQpO1xuICAgIGlmICghY29udGV4dCkgdGhyb3cgbmV3IEVycm9yKCdJbnZhbGlkIGNvbnRleHQuJyk7XG5cbiAgICByZXR1cm4gY29udGV4dDtcbiAgfVxuXG4gIHByaXZhdGUgZGVzZXJpYWxpemUgPSAobWV0aG9kOiBzdHJpbmcsIG9iamVjdDogYW55KSA9PiB7XG4gICAgaWYgKCF0aGlzLndlYkdMQ29udGV4dCB8fCBvYmplY3QgPT0gdW5kZWZpbmVkKSByZXR1cm4gb2JqZWN0OyAvL2Rlc2VyaWFsaXphdGlvbiBvbmx5IG5lZWRzIHRvIGhhcHBlbiBmb3Igd2ViR0xcblxuICAgIGlmIChvYmplY3QuaGFzT3duUHJvcGVydHkoXCJ3ZWJHTFR5cGVcIikgJiYgb2JqZWN0Lmhhc093blByb3BlcnR5KFwiaWRcIikpIHtcbiAgICAgIHJldHVybiAodGhpcy53ZWJHTE9iamVjdFtvYmplY3RbXCJpZFwiXV0pO1xuICAgIH0gZWxzZSBpZiAoQXJyYXkuaXNBcnJheShvYmplY3QpICYmICFtZXRob2QuZW5kc1dpdGgoXCJ2XCIpKSB7XG4gICAgICByZXR1cm4gSW50OEFycmF5Lm9mKC4uLihvYmplY3QgYXMgbnVtYmVyW10pKTtcbiAgICB9IGVsc2UgaWYgKHR5cGVvZihvYmplY3QpID09PSBcInN0cmluZ1wiICYmIChtZXRob2QgPT09IFwiYnVmZmVyRGF0YVwiIHx8IG1ldGhvZCA9PT0gXCJidWZmZXJTdWJEYXRhXCIpKSB7XG4gICAgICBsZXQgYmluU3RyID0gd2luZG93LmF0b2Iob2JqZWN0KTtcbiAgICAgIGxldCBsZW5ndGggPSBiaW5TdHIubGVuZ3RoO1xuICAgICAgbGV0IGJ5dGVzID0gbmV3IFVpbnQ4QXJyYXkobGVuZ3RoKTtcbiAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgbGVuZ3RoOyBpKyspIHtcbiAgICAgICAgICBieXRlc1tpXSA9IGJpblN0ci5jaGFyQ29kZUF0KGkpO1xuICAgICAgfVxuICAgICAgcmV0dXJuIGJ5dGVzO1xuICAgIH0gZWxzZVxuICAgICAgcmV0dXJuIG9iamVjdDtcbiAgfVxuXG4gIHByaXZhdGUgc2VyaWFsaXplID0gKG9iamVjdDogYW55KSA9PiB7XG4gICAgaWYgKG9iamVjdCBpbnN0YW5jZW9mIFRleHRNZXRyaWNzKSB7XG4gICAgICAgIHJldHVybiB7IHdpZHRoOiBvYmplY3Qud2lkdGggfTtcbiAgICB9XG5cbiAgICBpZiAoIXRoaXMud2ViR0xDb250ZXh0IHx8IG9iamVjdCA9PSB1bmRlZmluZWQpIHJldHVybiBvYmplY3Q7IC8vc2VyaWFsaXphdGlvbiBvbmx5IG5lZWRzIHRvIGhhcHBlbiBmb3Igd2ViR0xcblxuICAgIGNvbnN0IHR5cGUgPSB0aGlzLndlYkdMVHlwZXMuZmluZCgodHlwZSkgPT4gb2JqZWN0IGluc3RhbmNlb2YgdHlwZSk7XG4gICAgaWYgKHR5cGUgIT0gdW5kZWZpbmVkKSB7XG4gICAgICBjb25zdCBpZCA9IHRoaXMud2ViR0xPYmplY3QubGVuZ3RoO1xuICAgICAgdGhpcy53ZWJHTE9iamVjdC5wdXNoKG9iamVjdCk7XG5cbiAgICAgIHJldHVybiB7XG4gICAgICAgIHdlYkdMVHlwZTogdHlwZS5uYW1lLFxuICAgICAgICBpZDogaWRcbiAgICAgICAgfTtcbiAgICB9IGVsc2VcbiAgICAgIHJldHVybiBvYmplY3Q7XG4gIH1cbn1cbiJdLCJzb3VyY2VSb290IjoiIn0=