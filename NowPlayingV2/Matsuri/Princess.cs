using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowPlayingV2.Matsuri
{
    class Princess
    {
        /*
         *
             .,.      .(((,
            .MMMF      MMM!
           .MM#^ dHHHHHMMMMHHHHH`
          .M#^g&.      MMM_
         ?"  .MMMTMM#WMM"MM#WMMF        `      `       `     `   `   `   `      `   `   `      `   `   `      `   `   `
            .MM@`,MM],MN MMF.MMF           `      `  `    `                 `              `              `              `  `  `  `  `  `  `   `
     `     .MMM] .MMb.MN.MMR.MMF     `                      ..........                                 ..(i                                       `
         .J#MMM]      Jg.,                     `       .<1<>-.(>>>?><`` z1..    `   `          `  ..J1..(j{   `   `
         "` dMM]  .....JMM#!..                           ???7<&<_<???<.(?=jww++++((...   ` ......z<>>>>?j\            `
            dMM]  d@,MM# ^ .JMe         `   `                  ?, ======v!?<7XVC1CO&x1zVC>``.+<<<<?????j'                                     `
            dMM].dMF.MM#  .N MMN,                                JGx=llv```.1r _+??j0uAwuwOz=>```.??<<c`
    `       dMM]?TM].MMMMMM# ?9^                 `   `        .(i+C(vTC11??1zC(.+&ukzl=z77TO1=1+???`.!       `   `      `  `  `  `  `  `  `       `
       `    """^      ?777!          `                       ,jwzv1>>>>?????????????=vTi.``(l====ud=                `
                                        `  `  `         `  .Ov>;;>>>>?>????????=====???==OjeAXWV1??G.                                        `
    `      `dNNN   .....   MMM#                   `  `    ,>;;;>>>>>????????==?=??======?=?vwl ?x?>>O.   `  `  `       `    `    `    `   `     `
        `   dMMN   ,MMM`   dMM#   `                     .>;;;;>>><<(??+<<~(?=====1v<1========Si(Jc???O.           `                                `
            dMMN   ,MMM`   dMM#      `                 .>;;<_>>>?~_(????1(1==1x==-1~(=========I,(wz???l              `    `    `
    `       dMMN   ,MMM`   dMM# `       `  `  `  `    .>;;>->+???+?Z???=Z===u$(4===1========~(=zwlS???1-                           `   `    `  `
            dMMN   ,MMM`   dMM#                     ` v;>>>>?Z????jD?==dD==1C` _1====lz=====<.+1=Udz???1    `  `                                  `
       `    dMM#   ,MMM`   dMM#      `               .I>>>>?jI???1Cw==uhk==v```` I==l=R=l==zz==+1=XZ???z-         `   `  `  `   `
   `       `dMM#   ,MMM`   dMM#  `                   J>+>???zI?=?d7j==v`(zz>`.``.,z==jkz=lldSl=====S==??l                           `   `  `
            MMMF   ,MMM`   dMM#         `  `  `  `  .O>dz???1S==13~_6=% .0z_``.``.I=lv(0l=ldk=l====vz=??z      `                              `  `
      `    .MM#    ,MMM`   dMM#                     IjzyI????vI=zi-gMsm,~-O.```.``(=v`.O=ld<Sl=yl===0==?z.  `      `         `   `
           JMM!    ,MMM_   dMM# `  `               .z+Zyk???=l(1z#3jgMm?> `.``.`.`j<-``(=z:?Xlz0=lzkX====[            `  `           `    `        `
   `    ` .MD              dMM#       `            .>?dWWkz==z.M9>(Ha.H]````.````..(-gQgA~ ~zlZ=l=dVk=Oz?r                                     `
         .B!               """"          `  `  `   (?OzWVHWs=z$H` MMNMM%.``.``.```` (dMmTN, jZl=ljVyD==k=%     `  `         `  `       `
                  `  `  `          `               R?OzyWWkS_=z/``?Z<zV``.`````.`.`.M,.H;?M&Z=ludWWW3==dz}           `            `       `  `    `
     `              ....        `                 ,$?zdyW>1Z/.`.`.`..`.```.`.```.``dMHMH\_Hs7<jWVWXRl==zI{              `            `
                    MMM]                `         JI??XyW.:1?..`.`.``.``.``.`.```..474XF`.M\.JVVyWOl===zw}    `
           .........MMMb.......                `  I0?=dyW-_(:`.`..`.`.```.````.`````__``._../~?WVVSl==lvw\
            ````````MMMF```````                  .1S==vHVW,.i.```.``.``.``.``.``.`..`....`.x?<.HVV$=z=l=X}        `
                    MMM]                         ,?O?==dkVWn....``.````.`....``.``.`.``.`.x~~_.WVWIldO(=d.            `
           ,9"""""""MMM#""""""9                  z?dz==vWkVVWkd,.``.``.`(Y7Tl```````.`.`.J>_..WVyWIdyk==d_
                    MMM]                `        I<jZ===dWWVyVWWz&+...` r:~~z.`.`.`.`. .>`..XVVyVRuyyW==d~
              ......MMM]                         I~(Sl=ll4VyW9lzzzllvo `!~:J'```.`.-OOOwXWHkVVyVWwVyyX<=z`
            (@^    ?MMMMMNa,.                   .z~_WZ=lz~OVHHHWHkkzllt.````` ....$~_zykkkVWkyVWWyyyy$_=z         `
           .@       MMMF?WMMMNe         `       .=(.XSl=_1-?WVVyVWWk=ldI-...?!..dI:_(zWWVVWHWkyyyVyyf_(=r
           .N,     .MMM!   7M#`                 ,==zyWk=l=lllXUWWU6ll=dm++<.`.C+yli+=lvWWyyyWHyyyWV>(+=z`             `
             ?TBMMMH9^                          .Z=zyyWSzl=l=ll=llzv77Tkf!`.JkkqHzz=l=llvVUUWUUUVl===1z!      `
          `                                      ?z=TVyyykzll=llzY?7.`.j`. J"7I:JWyWAzzzzz11uz====lzyV`
                                        `        .?71zZWWyyVWXQI..``(-` >`(_-=:(J.HVyY``````` 1zwXZV'             `
                                              `.^````` ??7THHf~.`(.`.(.`._nC:++?(fVf```` ` ````(f=
                                              ( ` ` ``._`.WVf (-``(.``_`..3J>_uJfVK!. ` ` `  ```1
          ....ggHM"""""MMMNa,           `    -`` ` ` `-(dyyX7,:o.````.` .(iz.CJffK<J~_ ``` ``  `.             `       `
          7!             (MMMN.             ,  .,```` wWyyf_!.2.``.` .::ju.?fXffW>(__ ` ` `` ``  _
                          .MMMN             :_<>`` `.dyZVY`.>v.``.._:::J((~_XVVf$+/!` `` ` `` `` _        `
                           MMMM            ,.('`` `.WZyX% ! /```._~~::J(<>(.fffY^``` ` `` ` `` `.!
                          .MMMF         ` ?-? ` ` (ZZyXb .:,!````_~~:jinddHWY! ``` `` ` `` ` `` ;                 `
                         .dMMM!        ./^ ` ```.dZyZyyVWWaJ;..``-~~(HffffW'``` `` `  .... ` `.r                      `
          `            ..MMM"      `.?```` `` `.C?1zTUUWWfppk.`.`._~(WWWUU%` ....~_<<_~!``` ` /(              `
                   ..JMM#"!        ( `` ` ` ``.3?????=?==ZUWWb`.````.1z==d; (.! ..__`??-~. ````._         `
               -""""?`          `  {` ``   ```(????=?==1gWHHHH-`.```..fWxZ~7uuc7!~j>?jw+....  ` }      `
          `                       .}.` .~~~~_.Z???==1uWVfWHHbWL`.``.``(WVWk=(Cu77i!~.1rk1c-?<- `:
                                 .>1_~.j~~~~~~O1zzuwWZyyfpWW@@H-`.```.`zVWv71v:~~::~<w1WrZc_(c_.!                     `
              .                   1?^(O?c::::~jzuuuHZZyVWHgH@@@b.`.```. 4WHkJ.`_~~~(jv(wOJzC(>(^                  `
          `  .MMM\       dMMN     .1, Y1?><-::(kzuuWyyyWmHM@@@HH,.`.```..4Vy\``` ~~cI}O<vrV+.=
             .MMM`       -MMM    /...1+- _-.w::XuuZZHyWqHMHHHM@Nh.``.```` 4S ```` ~<((((2?J>;                 `
             ,MMN        .MMM_   .:... ~((z~`??<WuuZZHHHMHbk@@@HH|.``.`````<.`.``` .~{(J%{ `              `
             -MM#        .MMM_    ??`,~(>_<+1.j+ORZZZZXHpbbH@@@@@H..``.`.``.(-`.```..}u\>>
          `  -MM#        .MMM:       /<.Z<. x+<?$?kZXUXppbbHH@HMXkL`.``.``.``.i`.``` {2 ((             `              `
             ,MMN        .MMM`      /` ~~~(.` OTR~XIlzWfppbHkHbKXHW/.````.```` 1 ````.'  `                        `
             .MMM-       JMM#      /```_~~~~??1= _j=ldVfpbbHqHHHQHbS..``.``.````1 `.`/
          `             .MMM'     /```` .~~~~J!   .z=XWfWbbmkgHHVHbbL`.`````.``. l``.`                        `
                       .MM#!    .>``.`.` ..(?      I=lllzWHHkmkHlXWId,.``.``.````(_`>              `
                    ..MM#^   .,^``.``.```.('       1l=lllllWbHkHZllllO..``.``.``.``.`                  `
          `      (dW9"`    ./~``.```````.-`        (lllllldHbHkbRlllllG..`````.``.`/                                  `
                         .(~``.``.`.`.`./         .OzzzzzdHWk%zbkytllllw,.`.```.``.6,          `              `   `
                        ,'`````.````.`.>        .OllldyyWgHb=++WbWytttllZ& `.`.`..6llG,                   `
          `           ._``.`.`````.`.,`   .....OllllllZWHkkt`.<`?bbktttlllZn..`.JOllllzn.          `
                     .!``````.`.. ..oJOOI=l=zZIllllllOdbbW!v<> ..JbbRttttllttVVItltttlllvi             `
                    , `.`.`. .-vOz===lllllllllllllllldbpWC1z:1.>?-(WpHOtttttttttttttlttlllO,
          `       ._``.`..Jz1=====ll=llllllllllllllldbpWC:::(+~~` _?WpHtttttltttttOOtttttwwVn                     `   `
                 .^` .JI======lllllllllllllllllllltwHpW3:< ::?+ ```_dpWKtltttttttttZXOtOWfVyVk.               `
                ,~.JS======lllluOllllllllllllltltttXppK:<` ~~~?-`` `(HpHtttttttttttttZXtttZUUWV-..-++tOOl=1++(..
          `    !`,ZuS====ll=uZIlllllllllOZtztltttlOHpp$:~``.~:~<. ```dpWZtttttttttttttOvwtttttttOtOwXZXXXwwwzzWyW.
             .!`.,ZZW===lzs0v<+llllllltw0v<jttttttdbppI<``` ~~~~<`` `(ppKttttttttttrtrtt<Tyttttttttlv74XZZZZuZqkW]
            ,````.WyWzluZIz1JllllllttldIv~JtlttlttdbpbI(  `` ~~~~~```.ppHttttttrrtrrttrtr+?XOttttttttlz-?7TXXXqkk]
          .>```.`.4yVK6llllllllllttltZtv~(ttttttltdbpbc(-` `` ~~~_` ``XpWOtttttrttrtrrrrtto_?AtttttttttttO+-<7THk\
....     ,!``.``.`.K6=lllllllltttltOVtv~(tttttttttdHpbr~)`` `` ~~~.```dpbkttttrttrrrtrtrrrrO-?XyrttrtttttttttlzzlOu,
 ~`.<  .>``.```. (WWllllllllttltwAXStt>Jttttttttttdkbbr:1 `` `` ~~_` `(ppRtttrtrtrtrrtrrtrrrro(ZXrrrtrrtttttttltlldyyl
```` 1/`````.``..VVWZ=lllllttwwffW8ttOztttttttttttdHbbD:(. `` `` ~..``,ppHttttrtrtrtrtrrrrrrrrOJOXOrrrtrrtttttttttXyZk`
``.` ````.```..? WffpOllllOwXffpW8ttttttttttttttttwHbbb~(}` `` `` _.``.ppWrtttrtrrtrrrrrrrrrrrrrrrZyrrrrrrrrtttttwWyy%
``.`````.``..?`  ,WfpWzlOdffffpWHAwttrtrtrtttttttttHbbK~~1`` ` ` ` `-``WpWZttrtrtrrrrrrrrrrrrrrrrrvZkrrrrrrtrttttXfVW`
```.``.``` ,!     JWfpWdVVffppfHkkbbHmyrtrrttrtttttWkbH:~(- ` `` ```  `qpbRtttrrtrtrrtrrrrrrwAkWHkkkqkrrrrrrrrttdpfW\
`.``...+(-^     .IldWpWHHfppfpW@HHHbppWWyrtrrtrrrttdkbW<~~>``` `` ` ```(ppKttrtrrrrrrrrrrwdWpfpWWHHH@@Rrrrrrrrrdbpp%
--._?!        .vv<llvWmgg@kfppW@@@NHHWpfpHmrttttrtrdkkWl~~(.` ` `` ` ` ,ppHttrttrtrrrrwdppfWWHHbbWH@@@8rrrrrtrdbpp9i.
            .J=>(llllOWgggg@HkpH@@@@kWHkpffWWArrtttdqbkr~~_{`` ` `` ``` HpWOttrrrrrwQWbpbWHkbHbbH@@@HSvrrrrrAWbbWKtO<Ci.
           .Iv_JllllltZWHH@@@@@HkH@@@WbbHHWpbbkWAwrdHkbD~~.(`` `` `` ```dpWkrtrrrAXkkkHHHbbbbbHHMg@8vvvvvvwdkkkHHSrttz-?I,
          J=>(=lllllttttWpWH@@@@@@@@@HpbbbbHHHkqkkqHkbbb~~~_-`` `` `  ` (bpHwwAXHkkHHHbbbbbbbbWH@@SvvvrwQHkkqHHpWZrttttwwwS,
        .mXUXWAyllttlttttZWppWHH@@@MHbbpbpbbpbHHHHkkkkbR~~..(` ` `` ```` WppbbbbHHHbbbbbbbWWWWWJH@qqqqqqkqqHHHbWXrrrwdbpffVVh
       .WXkkXyVVWkttttttttwXppWZZOrrrrZUWbpbbppppH@gHUU=~..~_-` ` `` ` ``(UUUWH@HWUUUXrvvvvvvvvw(WHqqqqHmHHbbbSvrrwXHppWWHWqK
       XHyZyyWWWfpHstttrrwpppWSO1rrrrrrrrrZUWWpppH@gH```_....( `` ` ` `  ````(bbKrrrrrrrvvrvvrvvw/OdHbbbbbbb9zvvvAWkbWY<?4qk'
      .HpHkWWD:?WfppHyttdpppW8rIjrwwwrrrrrrrrrtOZHHHK`` ` ....~` ``` ``` ` ``.pbHrrrrrrrvvvrvvrvvwJOdHbkbbWUvzvvdHkk9<:~~~?t
        TpbHK::::?WppHmdppppHAXHWpffpfpbbWmAytttrkbpR` ``` ...__` ` `` `` ` ` WbWwrrrrrrrvvvvvvvzwAJwdHWUzzvzvzXkkH3::~~~~__>
          ?W3~~:::(4ppHHWppWHppWWWkkkkpppbbbbkHkXHppD`` ` ` ...(.` `` ` `` ```dpbRrrrrrrvvrvwQWHppbkkqHkvzzzzwXkkK>::~~~_ ``.;
           (```` _~:?WgmgHpWWHHppppppK:~~?7TWbbppppp$` ` ``  ...<`` `` `` `` `(pbRrrrrrrrAXHfpWWqHHHHHHHXvzzwWkqf<1:~~_``` ``1
           :` ` ``` ~(UgggHWMmmHWpppW%~~_______?7TYY'`` `` ``  .__` ` ```` `` .bpWwrrrwXHpfpYC:?HbbbHH@HUvwdHkq5?+(c:~```  ...,
          .` ``` `````-?HmmgHHmmqHHWK_````` {```````` `` `` ``` .(.` `  _  ````4ppWAgHpppV=::(I:?HpW@g@8zQqqkXC:<?C<:`.,?` ` ...<-
         */
    }
}
