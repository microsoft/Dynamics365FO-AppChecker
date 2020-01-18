% Copyright (c) Microsoft Corporation.
% Licensed under the MIT license.

% This is a set of clauses to demonstrate how information can be mined in prolog
% start the SWI prolog instance and load this file with 
% ?-[clauses].

classList(Package, L) :- findall(C, class(C, Package, _), L).
tableList(Package, L) :- findall(T, tabledef(T, Package, _), L).
formList(Package, L) :- findall(F, formdef(F, Package), L).
queryList(Package, L) :- findall(Q, querydef(Q, Package), L).

classescount(Package, Count) :- classList(Package, L), length(L,Count).
tablescount(Package, Count) :- tableList(Package, L), length(L,Count).
formscount(Package, Count) :- formList(Package, L), length(L,Count).
queriescount(Package, Count) :- queryList(Package, L), length(L,Count).

% toplevel artifacts that have an attribute
attributes(Package, Class, Attributes) :-
    class(Class, Package, _),
    findall(Attribute, attribute(Class, Attribute), Attributes).

attributes(Package, Table, Attributes) :-
    tabledef(Table, Package, _),
    findall(Attribute, attribute(Table, Attribute), Attributes).

attributes(Package, Form, Attributes) :-
    formdef(Form, Package),
    findall(Attribute, attribute(Form, Attribute), Attributes).

attributes(Package, Query, Attributes) :-
    querydef(Query, Package),
    findall(Attribute, attribute(Query, Attribute), Attributes).

% obsolete artifacts
obsolete(Package, C) :- attributes(Package,C,L), member(a(sysobsolete), L).

% obsolete methods
obsoletemethods(Package, Class, Method) :- methodattributes(Package, Class, Method, Attrs), member(a(sysobsolete), Attrs).

% List of Pairs of obsolete (Package, Artifact) in Package
obsoleteartifacts(Package, L) :- findall(cp(Package,C), obsolete(Package, C), L).

methodattributes(Package, Class, Method, Attrs) :-
    class(Class, Package, _),
    method(Class, Method, _),
    findall(A, methodattribute(Class, Method, A), Attrs).

methodattributes(Package, Table, Method, Attrs) :-
    tabledef(Table, Package, _),
    method(Table, Method, _),
    findall(A, methodattribute(Table, Method, A), Attrs).

methodattributes(Package, Form, Method, Attrs) :-
    formdef(Form, Package),
    method(Form, Method, _),
    findall(A, methodattribute(Form, Method, A), Attrs).

methodattributes(Package, Query, Method, Attrs) :-
    querydef(Query, Package),
    method(Query, Method, _),
    findall(A, methodattribute(Query, Method, A), Attrs).

% Pre handlers
prehandlers(P, c(C), m(M), PP, PC, PM) :-
    methodattributes(PP, PC, PM, Attrs),
    member(a(prehandlerfor, classstr(C), methodstr(C,M)), Attrs),
    class(c(C), P, _).

prehandlers(P, f(C), m(M), PP, PC, PM) :-
    methodattributes(PP, PC, PM, Attrs),
    member(a(prehandlerfor, formstr(C), formmethodstr(C,M)), Attrs),
    formdef(f(C), P).

prehandlers(P, t(C), m(M), PP, PC, PM) :-
    methodattributes(PP, PC, PM, Attrs),
    member(a(prehandlerfor, tablestr(C), tablemethodstr(C,M)), Attrs),
    tabledef(t(C), P ,_).

prehandlers(P, q(C), m(M), PP, PC, PM) :-
    methodattributes(PP, PC, PM, Attrs),
    member(a(prehandlerfor, querystr(C), querymethodstr(C,M)), Attrs),
    querydef(q(C), P).

% Post handlers
posthandlers(P, c(C), m(M), PP, PC, PM) :-
    methodattributes(PP, PC, PM, Attrs),
    member(a(posthandlerfor, classstr(C), methodstr(C,M)), Attrs),
    class(c(C), P, _).

posthandlers(P, f(C), m(M), PP, PC, PM) :-
    methodattributes(PP, PC, PM, Attrs),
    member(a(posthandlerfor, formstr(C), formmethodstr(C,M)), Attrs),
    formdef(f(C), P).

posthandlers(P, t(C), m(M), PP, PC, PM) :-
    methodattributes(PP, PC, PM, Attrs),
    member(a(posthandlerfor, tablestr(C), tablemethodstr(C,M)), Attrs),
    tabledef(t(C), P ,_).

posthandlers(P, q(C), m(M), PP, PC, PM) :-
    methodattributes(PP, PC, PM, Attrs),
    member(a(posthandlerfor, querystr(C), querymethodstr(C,M)), Attrs),
    querydef(q(C), P).